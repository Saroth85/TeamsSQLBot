using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// **Carica la configurazione dal file appsettings.json**
var configuration = builder.Configuration;
builder.Services.AddSingleton(configuration);

// ✅ Aggiunge il supporto per API Controllers
builder.Services.AddControllers(); // IMPORTANTE

builder.Services.AddSingleton<SQLHelper>();
builder.Services.AddSingleton<OpenAIHelper>();
builder.Services.AddTransient<IBot, TeamsBot>();
// ✅ Registra il Bot Framework
builder.Services.AddSingleton<BotFrameworkAuthentication>(sp =>
    new ConfigurationBotFrameworkAuthentication(configuration)
);

builder.Services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
builder.Services.AddTransient<IBot, TeamsBot>();
var app = builder.Build();

// ✅ Configura il middleware per gestire le richieste API
app.UseRouting();
//app.UseAuthorization();
app.MapControllers(); // IMPORTANTE

app.Run();
