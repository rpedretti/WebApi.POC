﻿using System;
using System.Threading.Tasks;
using WebApi.Shared.Models;

namespace WebApi.Client.Services
{
    public interface ISecurityService
    {
        Task<Tuple<string, string>> GetRsaKey(string keyPath);
        Task<ExchangePublicKeyModel> ExchangeRsaKey(string key);
        Task<ExchangePublicKeyModel> ExchangeTripleDesKey(string key, string rsaKey);
        Task<string> SendMessageOnSecureChannel(string message, UserAuthenticationModel userData);
        Task RequestJwtAsync(UserAuthenticationModel userData);
        Task UpdateJwtAsync(UserAuthenticationModel userData);
    }
}
