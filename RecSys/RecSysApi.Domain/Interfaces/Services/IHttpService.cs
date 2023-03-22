using System.Net.Http;
using System.Threading.Tasks;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Domain.Interfaces.Services
{
    public interface IHttpService
    {
        public Task<HttpResponseMessage> SendPostRequestToApiAsync<T>(RequestUrl<T> requestUrl);
        public Task<HttpResponseMessage> SendGetRequestToApiAsync<T>(RequestUrl<T> requestUrl);
    }
}