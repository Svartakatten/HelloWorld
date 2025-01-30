using Microsoft.Azure.Cosmos;

namespace Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateItem();
        }

        private static async Task CreateItem()
        {
            var cosmosUrl = "https://cosmosdbservices.documents.azure.com:443/";
            var databaseName = "PodManager";
            var containerName = "Podcasts";
            var partitionKeyPath = "/category";

            CosmosClient client = new CosmosClient(cosmosUrl, cosmosKey);

            Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath, 400);

            // Create a sample item
            var testItem = new
            {
                id = Guid.NewGuid().ToString(),
                partitionKey = "MyTestPkValue", // Match partition key definition
                details = "It's working"
            };

            var response = await container.CreateItemAsync(testItem, new PartitionKey(testItem.partitionKey));

            Console.WriteLine($"Item created with status code: {response.StatusCode}");
        }
    }
}
