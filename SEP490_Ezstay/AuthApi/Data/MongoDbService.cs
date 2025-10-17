using AuthApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi.Data;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    public MongoDbService( IOptions<MongoSettings> settings)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);

        var client = new MongoClient(clientSettings);
        _database = client.GetDatabase(settings.Value.DatabaseName);
      
    }

    public IMongoCollection<Account> Account => _database.GetCollection<Account>("Account");
    public IMongoCollection<EmailVerification> EmailVerifications => 
        _database.GetCollection<EmailVerification>("EmailVerification");
    public IMongoCollection<PhoneVerification> PhoneVerifications =>
    _database.GetCollection<PhoneVerification>("PhoneVerifications");

    public IMongoCollection<OwnerRegistrationRequest> OwnerRequests =>
    _database.GetCollection<OwnerRegistrationRequest>("OwnerRegistrationRequest");

}