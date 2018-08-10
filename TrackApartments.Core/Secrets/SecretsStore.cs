﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace TrackApartments.Core.Secrets
{
    public sealed class SecretsStore
    {
        private readonly string keyVaultUrl;
        private static readonly Dictionary<string, string> CachedDictionary = new Dictionary<string, string>();

        private static readonly HttpClient SharedClient = new HttpClient();

        public SecretsStore(string keyVaultUrl)
        {
            this.keyVaultUrl = keyVaultUrl;
        }

        private static readonly Lazy<KeyVaultClient> VaultClient = new Lazy<KeyVaultClient>(GetStore);

        public async Task<string> GetOrLoadSettingAsync(string secretId)
        {
            string value = String.Empty;

            if (CachedDictionary.ContainsKey(secretId))
            {
                value = CachedDictionary[secretId];
            }
            else
            {
                string tableString = (await VaultClient.Value.GetSecretAsync(keyVaultUrl, secretId)).Value;
                CachedDictionary[secretId] = tableString;
                value = tableString;
            }

            return value;
        }

        private static KeyVaultClient GetStore()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback),
                SharedClient);
            return keyVaultClient;
        }
    }
}
;