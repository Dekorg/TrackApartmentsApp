using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using TrackApartmentsApp.Core.Settings;

namespace TrackApartmentsApp.Domain.Secrets
{
    public class SecretsStore
    {
        private static readonly Dictionary<string, string> CachedDictionary = new Dictionary<string, string>();

        private static readonly HttpClient SharedClient = new HttpClient();

        private readonly AppSettings appSettings;

        public SecretsStore(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        private static readonly Lazy<KeyVaultClient> VaultClient = new Lazy<KeyVaultClient>(GetStore);

        public async Task<string> GetOrLoadSettingAsync(string secretId)
        {
            string value;

            if (CachedDictionary.ContainsKey(secretId))
            {
                value = CachedDictionary[secretId];
            }
            else
            {
                string tableString = (await VaultClient.Value.GetSecretAsync(appSettings.KeyVaultBaseUrl, secretId)).Value;
                CachedDictionary[secretId] = tableString;
                value = tableString;
            }

            return value;
        }

        private static KeyVaultClient GetStore()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback), SharedClient);
            return keyVaultClient;
        }
    }
}
;