using HouseLocationAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HouseLocationAPI.Data;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    public MongoDbService( IOptions<MongoSettings> settings)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);

        var client = new MongoClient(clientSettings);
        _database = client.GetDatabase(settings.Value.DatabaseName);
      
    }

    public IMongoCollection<HouseLocation> HouseLocations => _database.GetCollection<HouseLocation>("HouseLocations");
}