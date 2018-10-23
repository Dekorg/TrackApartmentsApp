using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TrackApartments.Core.Secrets
{
    public sealed class SecretsStore
    {
        private readonly string keyVaultUrl;
        private readonly string clientId;
        private readonly string clientSecret;

        private static readonly ConcurrentDictionary<string, string> CachedDictionary = new ConcurrentDictionary<string, string>();
        private static KeyVaultClient client;

        public SecretsStore(string keyVaultUrl, string clientId, string clientSecret)
        {
            this.keyVaultUrl = keyVaultUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;

            client = client ?? GetStore();
        }

        public async Task<string> GetOrLoadSettingAsync(string secretId)
        {
            if (!CachedDictionary.TryGetValue(secretId, out var value))
            {
                string tableString = (await client.GetSecretAsync(keyVaultUrl, secretId)).Value;
                CachedDictionary.TryAdd(secretId, tableString);
                value = tableString;
            }

            return value;
        }

        private KeyVaultClient GetStore()
        {
            var kv = new KeyVaultClient(GetToken);
            return kv;
        }

        public async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            var clientCred = new ClientCredential(clientId, clientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
;