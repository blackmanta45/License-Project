using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RecSysApi.Application.Commons.Extensions;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Services;

namespace RecSysApi.Infrastructure.Implementations.Services;

public class DigestAuthService : IDigestAuthService
{
    private string _cnonce;
    private int _nc;
    private string _nonce;
    private string _password;
    private string _qop;
    private string _realm;
    private string _user;

    public DigestAuthService()
    {
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        HttpClient.DefaultRequestHeaders.Connection.Add(HttpRequestHeader.KeepAlive.ToString());
    }

    public HttpClient HttpClient { get; }

    public async Task<HttpResponseMessage> SendGetRequestToUpvAsync<T>(RequestUrl<T> requestUrl, string user,
        string password)
    {
        _user = user;
        _password = password;
        var absoluteUri = $"{requestUrl.Protocol}://{requestUrl.Domain}/{requestUrl.Path}";
        var requestUri = absoluteUri.AddParameters(requestUrl.QueryParams);

        var httpRequestMessage = new HttpRequestMessage
        {
            RequestUri = requestUri
        };
        httpRequestMessage.AddHeaders(requestUrl.Headers);


        var response = await HttpClient.SendAsync(httpRequestMessage);
        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        var wwwAuthenticateHeader = response.Headers.WwwAuthenticate.ToString();
        _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
        _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
        _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);

        _nc = 0;
        _cnonce = new Random().Next(123400, 9999999).ToString();

        var httpRequestMessage2 = new HttpRequestMessage
        {
            RequestUri = requestUri,
            Headers = {{"Authorization", GetDigestHeader(requestUri.PathAndQuery)}}
        };

        response = await HttpClient.SendAsync(httpRequestMessage2);
        return response;
    }

    private string CalculateMd5Hash(
        string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hash = MD5.Create().ComputeHash(inputBytes);
        var sb = new StringBuilder();
        foreach (var b in hash)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    private string GrabHeaderVar(
        string varName,
        string header)
    {
        var regHeader = new Regex($@"{varName}=""([^""]*)""");
        var matchHeader = regHeader.Match(header);
        if (matchHeader.Success)
            return matchHeader.Groups[1].Value;
        throw new ApplicationException($"Header {varName} not found");
    }

    private string GetDigestHeader(
        string dir)
    {
        _nc = _nc + 1;

        var ha1 = CalculateMd5Hash($"{_user}:{_realm}:{_password}");
        var ha2 = CalculateMd5Hash($"{"GET"}:{dir}");
        var digestResponse =
            CalculateMd5Hash($"{ha1}:{_nonce}:{_nc:00000000}:{_cnonce}:{_qop}:{ha2}");

        return string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", " +
                             "algorithm=MD5, response=\"{4}\", qop={5}, nc={6:00000000}, cnonce=\"{7}\"",
            _user, _realm, _nonce, dir, digestResponse, _qop, _nc, _cnonce);
    }
}