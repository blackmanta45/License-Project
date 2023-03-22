using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecSysApi.Application.Commons.Extensions;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Services;

namespace RecSysApi.Infrastructure.Implementations.Services;

//Consider switching HttpClient to IHttpClientFactory
public sealed class HttpService : IHttpService
{
    private readonly ILogger<HttpService> _logger;

    public HttpService(ILogger<HttpService> logger)
    {
        _logger = logger;
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        HttpClient.DefaultRequestHeaders.Connection.Add(HttpRequestHeader.KeepAlive.ToString());
        HttpClient.Timeout = new TimeSpan(0,2,0,0); //2 hours
    }

    public HttpClient HttpClient { get; }

    public async Task<HttpResponseMessage> SendPostRequestToApiAsync<T>(RequestUrl<T> requestUrl)
    {
        var absoluteUri = $"{requestUrl.Protocol}://{requestUrl.Domain}/{requestUrl.Path}";
        var requestUri = absoluteUri.AddParameters(requestUrl.QueryParams);
        _logger.LogInformation($"Sending GET request to: {requestUri}");

        var signUpFormModelJson = JsonConvert.SerializeObject(requestUrl.Content);
        var requestContent = new StringContent(signUpFormModelJson, Encoding.UTF8, "application/json");


        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = requestUri,
            Content = requestContent
        };

        httpRequestMessage.AddHeaders(requestUrl.Headers);

        var response = await HttpClient.SendAsync(httpRequestMessage);

        return response;
    }

    public async Task<HttpResponseMessage> SendGetRequestToApiAsync<T>(RequestUrl<T> requestUrl)
    {
        var absoluteUri = $"{requestUrl.Protocol}://{requestUrl.Domain}/{requestUrl.Path}";
        var requestUri = absoluteUri.AddParameters(requestUrl.QueryParams);
        _logger.LogInformation($"Sending GET request to: {requestUri}");


        var signUpFormModelJson = JsonConvert.SerializeObject(requestUrl.Content);
        var requestContent = new StringContent(signUpFormModelJson, Encoding.UTF8, "application/json");

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = requestUri,
            Content = requestContent
        };
        httpRequestMessage.AddHeaders(requestUrl.Headers);

        var response = await HttpClient.SendAsync(httpRequestMessage);

        return response;
    }
}