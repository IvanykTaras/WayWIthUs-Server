using MongoDB.Driver;

namespace WayWIthUs_Server.Data
{
    public class MongoDbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;
        private readonly IMongoDatabase? _userDatabase;
        public MongoDbService(IConfiguration configuration) {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);

            var dbName = _configuration.GetConnectionString("DbName");
            _database = mongoClient.GetDatabase(dbName);

            var userdbName = _configuration.GetConnectionString("UserDbName");
            _userDatabase = mongoClient.GetDatabase(userdbName);
        }

        public IMongoDatabase? Database => _database;
        public IMongoDatabase? UserDatabase => _userDatabase;
    }
}
