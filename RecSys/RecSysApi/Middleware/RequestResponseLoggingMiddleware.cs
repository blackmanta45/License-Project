using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RecSysApi.Presentation.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //First, get the incoming request
        var request = await FormatRequest(context.Request);

        //Copy a pointer to the original response body stream
        var originalBodyStream = context.Response.Body;

        //Create a new memory stream...
        using (var responseBody = new MemoryStream())
        {
            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            //Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);

            //Format the response from the server
            var response = await FormatResponse(context.Response);
            var myResponse = await GetResponseBody(context.Response);

            //TODO: Save log to chosen datastore

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await myResponse.CopyToAsync(originalBodyStream).ConfigureAwait(false);
        }
        context.Response.Body = originalBodyStream;
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {

        //This line allows us to set the reader for the request back at the beginning of its stream.
        request.EnableBuffering();

        //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];

        //...Then we copy the entire request stream into the new buffer.
        await request.Body.ReadAsync(buffer, 0, buffer.Length);

        //We convert the byte[] into a string using UTF8 encoding...
        var bodyAsText = Encoding.UTF8.GetString(buffer);

        //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
        request.Body.Seek(0, SeekOrigin.Begin);

        return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
    }


    private async Task<Stream> GetResponseBody(HttpResponse response)
    {
        response.Headers.ContentType = new StringValues("application/json");

        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        JObject jObject = JObject.Parse(text);
        var id = (Guid)jObject["Id"];
        var message = (string)jObject["Message"];
        var statusCode = (string)jObject["StatusCode"];
        var content = JsonConvert.SerializeObject(jObject["Content"]);
        if (content == "null")
            content = string.Empty;
        else
        {
            content = content.First() == '"' ? content.Remove(0, 1) : content;
            content = content.Last() == '"' ? content.Remove(content.Length - 1, 1) : content;
        }

        var stream = new MemoryStream();
        var sw = new StreamWriter(stream);
        await sw.WriteAsync(content);
        await sw.FlushAsync();
        stream.Position = 0;

        return stream;
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        //We need to read the response stream from the beginning...
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();

        response.Body.Seek(0, SeekOrigin.Begin);
        
        //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
        return $"{response.StatusCode}: {text}";
    }
}