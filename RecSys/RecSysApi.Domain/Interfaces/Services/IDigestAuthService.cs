using System.Net.Http;
using System.Threading.Tasks;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Domain.Interfaces.Services
{
    public interface IDigestAuthService
    {
        public Task<HttpResponseMessage> SendGetRequestToUpvAsync<T>(RequestUrl<T> requestUrl, string user, string password);
    }
}