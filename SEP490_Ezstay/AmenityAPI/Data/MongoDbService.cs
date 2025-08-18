using AmenityAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AmenityAPI.Data;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    public MongoDbService( IOptions<MongoSettings> settings)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);

        var client = new MongoClient(clientSettings);
        _database = client.GetDatabase(settings.Value.DatabaseName);
      
    }

    public IMongoCollection<Amenity> Amenities => _database.GetCollection<Amenity>("Amenities");
}