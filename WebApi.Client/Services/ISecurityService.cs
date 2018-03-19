﻿using System.Threading.Tasks;
using WebApi.Shared.Models;

namespace WebApi.Client.Services
{
    public interface ISecurityService
    {
        Task<ExchangePublicKeyModel> ExchangeRsaKey(string key);
        Task<ExchangePublicKeyModel> ExchangeTripleDesKey(string key, string rsaKey);
        Task<string> SendMessageOnSecureChannel(string message);
        Task RequestJwtAsync(UserAuthenticationModel userData);
    }
}
