using BookAPI.Settings;
using BookAPI.Repositories;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

// Configurează serializarea pentru Guid
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

var builder = WebApplication.CreateBuilder(args);

// Citește configurația MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Înregistrează `IMongoDbSettings`
builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

// Configurează serviciul MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IMongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});

// Înregistrează IMongoDatabase
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IMongoDbSettings>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Înregistrează repository-ul generic
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// Adaugă controlere
builder.Services.AddControllers();

var app = builder.Build();

// Activează maparea controlerelor
app.MapControllers();

app.Run();
