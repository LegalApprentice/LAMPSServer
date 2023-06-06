namespace LAMPSServer.Helpers;

using System;
using System.Threading.Tasks;
using Elasticsearch.Net;

//https://gist.github.com/stevejgordon/dd5b43ce4a9654d67a77f6983cec8f32

// Using C# 9 record to define our book DTO
public record Book(int Id, string Title, string ISBN);

public class Simple
{

    public async void SimpleTest()
    {

        // defaults to localhost:9200
        var client = new ElasticLowLevelClient();

        var json = "{\"Id\":1,\"Title\":\"Pro .NET Memory Management\",\"ISBN\":\"978-1-4842-4026-7\"}";

        // index a document from a JSON string, creating an index with auto-mapped properties
        var indexResponse = await client.IndexAsync<StringResponse>("books", "1", PostData.String(json));

        if (indexResponse.Success)
        {
            await Task.Delay(10);

            // after a short delay, try to search for a book with a specific ISBN
            var searchResponse = await client.SearchAsync<StringResponse>("books", PostData.Serializable(new
            {
                query = new
                {
                    match = new
                    {
                        isbn = new
                        {
                            query = "978-1-4842-4026-7"
                        }
                    }
                }
            }));

            if (searchResponse.Success)
            {
                Console.WriteLine(searchResponse.Body);
                Console.WriteLine();
            }
        }


        var bookToIndex = new Book(2, "Pro .NET Benchmarking", "978-1-4842-4940-6");

        // index another book, this time serializing an object
        indexResponse = await client.IndexAsync<StringResponse>("books", bookToIndex.Id.ToString(), PostData.Serializable(bookToIndex));

        if (indexResponse.Success)
        {
            await Task.Delay(10);

            // after a short delay, get the book back by its ID
            var searchResponse = await client.GetAsync<DynamicResponse>("books", bookToIndex.Id.ToString());

            if (searchResponse.Success)
            {
                // access the title by path notation from the dynamic response
                Console.WriteLine($"Title: {searchResponse.Get<string>("_source.Title")}");
            }
        }

        // clean up by removing the index
        await client.Indices.DeleteAsync<VoidResponse>("books");
    }
}





