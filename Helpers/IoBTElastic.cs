namespace LAMPSServer.Helpers;
using Elasticsearch.Net;
using LAMPSServer.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/

// https://github.com/elastic/elasticsearch-net 
// https://csharp.hotexamples.com/examples/Nest/ElasticClient/Search/php-elasticclient-search-method-examples.html



public class SearchSpec
{
    public string Includeany { get; set; }
}

public class QuerySpec: SearchSpec
{
    public string Includeall { get; set; }
    public string Exactphrase { get; set; }
    public string Excludeany { get; set; }
    public string[] ClassFilter { get; set; }
    public string[] TagsFilter { get; set; }
}



public interface IElasticWrapper
{
    ElasticLowLevelClient LowClient();
    ElasticClient Client();

    ElasticWrapper WriteToDB(string index, object data);
    List<T> ReadIndex<T>(string index) where T : LABase;

    Uri ElasticSearchURL();
    bool HealthCheck();
    bool SmokeTest();

    StringResponse FindCase(string index, string caseid);
    StringResponse FindParagraph(string index, string paragraphid);
    StringResponse FindSentence(string index, string sentenceid);

    List<LASearchResult<LASentence>> FilterTesting(string index);

    List<LASearchResult<LASentence>> QueryForSentence(string index, QuerySpec spec);
}


//https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-use-dom-utf8jsonreader-utf8jsonwriter?pivots=dotnet-6-0#deserialize-subsections-of-a-json-payload



public class ElasticWrapper : IElasticWrapper
{
    private static int id = 0;

    private IEnvConfig _envconfig { get; init;  }
    public ElasticClient client { get; set; }
    
    public ElasticLowLevelClient lowClient { get; set; }

    public ElasticWrapper(IEnvConfig config)
    {
        _envconfig = config;

        var url = ElasticSearchURL();
        var esConfiguration = new ConnectionSettings(url);
        client = new ElasticClient(esConfiguration);
        lowClient = new ElasticLowLevelClient(esConfiguration);
    }

    // public ElasticWrapper(string url)
    // {
    //     var esConfiguration = new ConnectionSettings(new Uri(url));
    //     client = new ElasticClient(esConfiguration);
    //     lowClient = new ElasticLowLevelClient(esConfiguration);
    // }

    public Uri ElasticSearchURL()
    {
        var url = new Uri(_envconfig.LAdata_URL());
        return url;
    }

    public ElasticLowLevelClient LowClient()
    {
        return lowClient;  
    }

    public ElasticClient Client()
    {
        return client;
    }

    public List<T> ReadIndex<T>(string index) where T: LABase
    {

        $"env indexName= {index.ToLower()}".WriteLine();

        var searchResponse = Client().Search<T>(s => s
            .Index(index.ToLower())
            .Size(10000)
            .MatchAll());

        var list = new List<T>();
        var docs = searchResponse.Documents.GetEnumerator();
        while (docs.MoveNext())
        {
            list.Add(docs.Current);
        }
        return list;

    }


    public ElasticWrapper WriteToDB(string index, object data)
    {
        try
        {
            // $"..................................".WriteLine(ConsoleColor.Magenta);
            $"WriteToDB:{index}: data {data}".WriteLine(ConsoleColor.Magenta);
 
            id++;
            var indexResponse = LowClient().Index<StringResponse>(index.ToLower(), id.ToString(), PostData.String(data.ToString()));

            if (indexResponse.Success == false)
            {
                $"ERROR WriteToDB {indexResponse.ApiCall.DebugInformation}".WriteLine(ConsoleColor.Yellow);
            }
        }
        catch (Exception ex)
        {
            $"ERROR WriteToDB:{index}: {ex.Message}".WriteLine(ConsoleColor.Magenta);
        }
        return this;
    }

    public object MatchCaseID(string id)
    {
        return new { match = new { caseNumber = new { query = id } } };
    }

    public StringResponse FindCase(string index, string caseid)
    {
        try
        {
            var q = new
            {
                from = 0,
                size = 3,
                query = MatchCaseID(caseid)
            };

            var result = LowClient().Search<StringResponse>(index, PostData.Serializable(q));

            return result;
        }
        catch (Exception ex)
        {
            return new StringResponse(ex.Message);
        }       
    }

    public bool HealthCheck()
    {
        return true;
    }

    public bool SmokeTest()
    {
        return true;
    }


    public object MatchParagraphID(string id)
    {
        return new { match = new { paraID = new { query = id } } };
    }

    public StringResponse FindParagraph(string index, string paragraphid)
    {
        try
        {
            var q = new
            {
                from = 0,
                size = 3,
                query = MatchParagraphID(paragraphid)
            };

            var result = LowClient().Search<StringResponse>(index, PostData.Serializable(q));

            return result;
        }
        catch (Exception ex)
        {
            return new StringResponse(ex.Message);
        }
    }

    public object MatchSentenceID(string id)
    {
        return new { match = new { sentID = new { query = id } } };
    }

    public StringResponse FindSentence(string index, string sentenceid)
    {
        try
        {
            var q = new
            {
                from = 0,
                size = 3,
                query = MatchSentenceID(sentenceid)
            };

            var result = LowClient().Search<StringResponse>(index, PostData.Serializable(q));

            return result;
        }
        catch (Exception ex)
        {
            return new StringResponse(ex.Message);
        }
    }


  
    //  https://www.methylium.com/articles/elastic-search-filters/


    //        ...
    //}
    /*
        {
      "includeany": "gun",
      "includeall": "string",
      "exactphrase": "string",
      "excludeany": "string",
      "classFilter": [ "FactSentence"
      ],
      "tagsFilter": [
      ]
    }


    */

    //https://stackoverflow.com/questions/38014661/combining-boolquerydescriptor-in-nest-2-x

    //https://www.methylium.com/articles/elastic-search-filters/
    //https://www.csharpcodi.com/csharp-examples/Nest.QueryContainerDescriptor.Term(System.Func)/
    //https://csharp.hotexamples.com/examples/Nest/ElasticClient/Search/php-elasticclient-search-method-examples.html
    
    
    public BoolQueryDescriptor<T> CombineBoolQueryDescriptors<T>(List<BoolQueryDescriptor<T>> queries) where T : LABase
    {
        var descriptor = new BoolQueryDescriptor<T>();
        var combinedQuery = (IBoolQuery)descriptor;

        foreach (var query in queries.Cast<IBoolQuery>())
        {
            if (query.Must != null)
            {
                combinedQuery.Must = combinedQuery.Must != null
                    ? combinedQuery.Must.Concat(query.Must)
                    : (query.Must.ToArray());
            }
            if (query.Should != null)
            {
                combinedQuery.Should = combinedQuery.Should != null
                    ? combinedQuery.Should.Concat(query.Should)
                    : (query.Should.ToArray());
            }

            if (query.MustNot != null)
            {
                combinedQuery.MustNot = combinedQuery.MustNot != null
                    ? combinedQuery.MustNot.Concat(query.MustNot)
                    : (query.MustNot.ToArray());
            }

            if (query.Filter != null)
            {
                combinedQuery.Filter = combinedQuery.Filter != null
                    ? combinedQuery.Filter.Concat(query.Filter)
                    : (query.Filter.ToArray());
            }
        }
        return descriptor;
    }






    public List<LASearchResult<LASentence>> QueryForSentence(string index, QuerySpec spec)
    {
        var queries = new List<BoolQueryDescriptor<LASentence>>();

        $"env indexName= {index.ToLower()}".WriteLine();


        if (!string.IsNullOrEmpty(spec.Includeany) ) {
            var Includeany = new BoolQueryDescriptor<LASentence>();
            Includeany.Must(m => m.QueryString(queryString => queryString
                        .DefaultOperator(Operator.Or).Query(spec.Includeany)));
            queries.Add(Includeany);
        }

        if (!string.IsNullOrEmpty(spec.Includeall) ) {
            var Includeall = new BoolQueryDescriptor<LASentence>();
            Includeall.Must(m => m.QueryString(queryString => queryString
                        .DefaultOperator(Operator.And).Query(spec.Includeall)));
            queries.Add(Includeall);
        }

        if (!string.IsNullOrEmpty(spec.Excludeany) ) {
            var Excludeany = new BoolQueryDescriptor<LASentence>();
            Excludeany.MustNot(m => m.QueryString(queryString => queryString
                        .DefaultOperator(Operator.Or).Query(spec.Excludeany)));
            queries.Add(Excludeany);
        }

        if (!string.IsNullOrEmpty(spec.Exactphrase) ) {
            var phrase = new QueryContainerDescriptor<LASentence>();
            phrase.MatchPhrase(m => m.Field(f => f.text).Query(spec.Exactphrase));
            var ExactPhrase = new BoolQueryDescriptor<LASentence>();
            ExactPhrase.Must(m => phrase);
            queries.Add(ExactPhrase);
        }


        //https://stackoverflow.com/questions/37697866/nest-conditional-filter-query-with-multiple-terms

        //https://stackoverflow.com/questions/37697866/nest-conditional-filter-query-with-multiple-terms

        if (spec.ClassFilter.Length > 0)
        {
            //this works  GREAT!!
            var matchany = String.Join(' ', spec.ClassFilter);
            var ClassFilters = new BoolQueryDescriptor<LASentence>();
            ClassFilters.Must(m =>m
                            .Match(mt => mt
                                .Field(f => f.rhetClass).Query(matchany).Operator(Operator.Or)
                            )
            );
            queries.Add(ClassFilters);
        }

        //{ "filter":[{ "terms":{ "rhetClass":["Fact"]} }],"must":[{ "query_string":{ "default_operator":"or","query":"gun"} }]}

        //https://spoon-elastic.com/all-elastic-search-post/advanced-usage/boolean-query-with-elasticsearch-influence-elasticsearch-scoring-part-1/

        var descriptor = CombineBoolQueryDescriptors<LASentence>(queries);
        var json = client.RequestResponseSerializer.SerializeToString(descriptor);

        var searchResponse = client.Search<LASentence>(s => s
             .From(0)
             .Size(10000)
             .Explain(true)
             .TrackScores(true)
             .Index(index.ToLower())
             .Query(q => q.Bool(b => descriptor))
             );


        var list = new List<LASearchResult<LASentence>>();

        var hits = searchResponse.Hits.GetEnumerator();
        while (hits.MoveNext())
        {
            var hit = hits.Current;

            list.Add(new LASearchResult<LASentence>()
            {
                _index  = hit.Index,
                _type = hit.Type,
                _score = hit.Score,
                _id = hit.Id,
                _source = hit.Source
            });
        }
        return list;

    }

    public List<LASearchResult<LASentence>> FilterTesting(string index)
    {
        var queries = new List<BoolQueryDescriptor<LASentence>>();

        $"env indexName= {index.ToLower()}".WriteLine();

        //https://stackoverflow.com/questions/37697866/nest-conditional-filter-query-with-multiple-terms
        //https://stackoverflow.com/questions/37697866/nest-conditional-filter-query-with-multiple-terms
        //https://spoon-elastic.com/all-elastic-search-post/advanced-usage/boolean-query-with-elasticsearch-influence-elasticsearch-scoring-part-1/


        var xxx = new[] { "ReasoningSentence" };  //"FactSentence" , 
        var Includeany = String.Join(' ', xxx);



        var searchResponse = client.Search<LASentence>(s => s
             .From(0)
             .Size(10000)
             .Index(index.ToLower())
             .Query(q => q
               .Bool(b => b
                   .Must(m =>
                       m.Match(mt1 => mt1.Field(f1 => f1.rhetClass).Query(Includeany).Operator(Operator.Or))))
            ));


        var list = new List<LASearchResult<LASentence>>();

        var hits = searchResponse.Hits.GetEnumerator();
        while (hits.MoveNext())
        {
            var hit = hits.Current;

            list.Add(new LASearchResult<LASentence>()
            {
                _index = hit.Index,
                _type = hit.Type,
                _score = hit.Score,
                _id = hit.Id,
                _source = hit.Source
            });
        }
        return list;

    }

}


