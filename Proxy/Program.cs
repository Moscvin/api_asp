using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Adaugă fișierul de configurare `ocelot.json`
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Adaugă serviciile Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

// Mapare middleware pentru Ocelot
app.UseOcelot().Wait();

app.Run();
