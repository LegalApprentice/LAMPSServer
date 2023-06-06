namespace LAMPSServer.Helpers;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


using System.Text;
using System.Web;

public static class HttpRequestExtensions
{
    //create in a static class
    static public object GetPropertyValue(this object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
    }



    public static Uri GetCompleteUri(this HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(80),
            Path = request.Path.ToString(),
            Query = request.QueryString.ToString()
        };
        return uriBuilder.Uri;
    }

    public static Uri GetServerUri(this HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(80),
        };
        return uriBuilder.Uri;
    }

    public static Uri GetRefUri(this HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            //Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port.GetValueOrDefault(80),
        };
        return uriBuilder.Uri;
    }

}
public static class ConsoleHelpers
{
    public static void WriteLine<T>(this T entity, ConsoleColor? color = null)
    {
        if (color.HasValue)
            Console.ForegroundColor = color.Value;
        Console.WriteLine(entity != null ? JsonConvert.SerializeObject(entity, Formatting.Indented) : "null");
        Console.ResetColor();
    }

    public static void WriteLine(this string message, ConsoleColor? color = null)
    {
        if (color.HasValue)
            Console.ForegroundColor = color.Value;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Write(this string message, ConsoleColor? color = null)
    {
        if (color.HasValue)
            Console.ForegroundColor = color.Value;
        Console.Write(message);
        Console.ResetColor();
    }

    public static void WriteInLine(this string message, ConsoleColor? color = null)
    {
        if (color.HasValue)
            Console.ForegroundColor = color.Value;
        Console.Write(message.Trim() + " "); //make sure there's a space at the end of the message since it's inline.
        Console.ResetColor();
    }

}

public static partial class Extensions
{
    /// <summary>
    ///     Encodes a URL string.
    /// </summary>
    /// <param name="str">The text to encode.</param>
    /// <returns>An encoded string.</returns>
    public static String UrlEncode(this String str)
    {
        return HttpUtility.UrlEncode(str);
    }

    /// <summary>
    ///     Encodes a URL string using the specified encoding object.
    /// </summary>
    /// <param name="str">The text to encode.</param>
    /// <param name="e">The  object that specifies the encoding scheme.</param>
    /// <returns>An encoded string.</returns>
    public static String UrlEncode(this String str, Encoding e)
    {
        return HttpUtility.UrlEncode(str, e);
    }
}
