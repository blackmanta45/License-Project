using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecSysApi.Application.Commons.Settings;
using RecSysApi.Application.Interfaces;
using RecSysApi.Domain.Entities;
using RecSysApi.Domain.Interfaces.Repositories;
using RecSysApi.Domain.Models;

namespace RecSysApi.Application.Services;

public sealed class LoginService : ILoginService
{
    private readonly AppSettings _appSettings;
    private readonly IUserRepository _userRepository;

    public LoginService(IUserRepository userRepository,
        IOptions<AppSettings> appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<CustomResponse<string>> Authenticate(UserLogin login)
    {
        var dbUser = await _userRepository.GetUserByUsername(login.Username);
        if (dbUser == null)
            return new CustomResponse<string>
            {
                Status = HttpStatusCode.Unauthorized,
                Content = "Username or password are incorrect"
            };

        var token = GenerateJwtToken(dbUser);
        if (CheckMatch(dbUser.Hash, login.Password))
            return new CustomResponse<string>
            {
                Status = HttpStatusCode.OK,
                Content = token
            };

        return new CustomResponse<string>
        {
            Status = HttpStatusCode.Unauthorized,
            Content = "Username or password are incorrect"
        };
    }


    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", user.Id.ToString())}),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private bool CheckMatch(string hash, string input)
    {
        try
        {
            var parts = hash.Split(':');

            var salt = Convert.FromBase64String(parts[0]);

            var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return parts[1].Equals(Convert.ToBase64String(bytes));
        }
        catch
        {
            return false;
        }
    }

    public string CalculateHash(string input)
    {
        var salt = GenerateSalt(16);

        var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(bytes)}";
    }

    private static byte[] GenerateSalt(int length)
    {
        var salt = new byte[length];

        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(salt);
        }

        return salt;
    }
}