using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data
{
    public class CosmosDbContext : DbContext
    {
        private readonly string _cosmosEndpoint;
        private readonly string _cosmosKey;
        private readonly string _databaseName;

        public CosmosDbContext(DbContextOptions<CosmosDbContext> options, string cosmosEndpoint, string cosmosKey, string databaseName)
            : base(options)
        {
            _cosmosEndpoint = cosmosEndpoint;
            _cosmosKey = cosmosKey;
            _databaseName = databaseName;
        }

        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(_cosmosEndpoint, _cosmosKey, _databaseName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .ToContainer("Users")
                .HasPartitionKey(u => u.Category);
        }
    }
}