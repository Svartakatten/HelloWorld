using Microsoft.Azure.Cosmos;
using HelloWorld.Resources;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloWorld
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CosmosDbContext>();

            await context.Database.EnsureCreatedAsync(); // Ensure database is created

            // Create and retrieve users
            await CreateUser(context);
            await FetchUsers(context);

            await host.RunAsync();
        }

        private static async Task CreateUser(CosmosDbContext context)
        {
            var newUser = new Users
            {
                Username = "JohnDoe",
                Email = "johndoe@example.com",
                PasswordHash = "hashedpassword123",
                Role = "Admin",
                Category = "general"
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            Console.WriteLine($"User {newUser.Username} created successfully!");
        }
        private static async Task FetchUsers(CosmosDbContext context)
        {
            var users = await context.Users.ToListAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Username}, Email: {user.Email}");
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(async (hostContext, services) =>
                {
                    string vaultName = "podmanagersecure";
                    string secretName = "CosmosDbSecure";
                    string cosmosDbKey = await KeyVaultConnection.GetSecretValueAsync(vaultName, secretName);

                    string cosmosDbEndpoint = "https://cosmosdbservices.documents.azure.com:443/";
                    string databaseName = "PodManager";

                    services.AddDbContext<CosmosDbContext>(options =>
                        options.UseCosmos(cosmosDbEndpoint, cosmosDbKey, databaseName));
                });
    }
}
