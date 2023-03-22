using System.Threading.Tasks;
using RecSysApi.Domain.Entities;

namespace RecSysApi.Application.Interfaces;

public interface ILoginService
{
    Task<CustomResponse<string>> Authenticate(UserLogin login);
    string CalculateHash(string input);
}