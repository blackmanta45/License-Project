using System.Collections.Generic;
using System.Net.Http;

namespace RecSysApi.Application.Commons.Extensions;

public static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage AddHeaders(this HttpRequestMessage httpRequestMessage,
        Dictionary<string, string> headers)
    {
        foreach (var (key, value) in headers) httpRequestMessage.Headers.Add(key, value);

        return httpRequestMessage;
    }
}