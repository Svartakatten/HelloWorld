using Microsoft.Azure.Cosmos;
using Application.Resources;

namespace Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var secretValue = await KeyVaultConnection.GetSecretValueAsync("podmanagersecure", "CosmosDbSecure");
            Console.WriteLine($"Secret Value: {secretValue}");

            await CreateItem(secretValue);
        }

        private static async Task CreateItem(string cosmosKey)
        {
            var cosmosUrl = "https://cosmosdbservices.documents.azure.com:443/";
            var databaseName = "PodManager";
            var containerName = "Podcasts";
            var partitionKeyPath = "/category";

            CosmosClient client = new CosmosClient(cosmosUrl, cosmosKey);

            Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath, 400);

            var testItem = new
            {
                id = Guid.NewGuid().ToString(),
                partitionKey = "MyTestPkValue",
                details = "It's working"
            };

            var response = await container.CreateItemAsync(testItem, new PartitionKey(testItem.partitionKey));

            Console.WriteLine($"Item created with status code: {response.StatusCode}");
        }
    }
}
