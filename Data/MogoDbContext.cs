using MongoDB.Driver;
using DrugSystem.Models;

namespace DrugSystem.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("drugSystem");
        }

        public IMongoCollection<Pharmacy> Pharmacies => _database.GetCollection<Pharmacy>("Pharmacies");

        public IMongoCollection<Drug> Drugs => _database.GetCollection<Drug>("Drugs");

    }
}
