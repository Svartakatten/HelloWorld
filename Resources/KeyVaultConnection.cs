using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace HelloWorld.Resources
{
    public static class KeyVaultConnection
    {
        public static async Task<string> GetSecretValueAsync(string vaultName, string secretName)
        {
            var kvUri = $"https://{vaultName}.vault.azure.net/";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            KeyVaultSecret secret = await client.GetSecretAsync(secretName);

            return secret.Value;
        }
    }
}
