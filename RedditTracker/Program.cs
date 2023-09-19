using Microsoft.Extensions.Options;
using RedditTracker;
using RedditTracker.Models.Configurations;
using RedditTracker.ServiceContracts;
using RedditTracker.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

//Add Configurations
services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
services.Configure<LogSettings>(configuration.GetSection("LogSettings"));
services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injections

services.AddTransient<ISubRedditService, SubRedditService>();
services.AddTransient<ISqlService, SqlService>();
services.AddSingleton<IEmailSettings>(services => services.GetRequiredService<IOptions<EmailSettings>>().Value);
services.AddSingleton<ILogSettings>(services => services.GetRequiredService<IOptions<LogSettings>>().Value);
services.AddSingleton<IAppSettings>(services => services.GetRequiredService<IOptions<AppSettings>>().Value);
services.AddSingleton<ISubRedditManagerService, SubRedditManagerService>();
services.AddHttpClient<IRedditHttpClient, RedditHttpClient>();
services.AddHttpClient<IRedditAuthHttpClient, RedditAuthHttpClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiAuthetication>();
app.UseMiddleware<ApiExceptionHandler>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var redditHttpClient = app.Services.GetRequiredService<IRedditHttpClient>();
var subRedditManagerService = app.Services.GetRequiredService<ISubRedditManagerService>();

var technologyTask = new SubRedditTask("technology", redditHttpClient, subRedditManagerService);
technologyTask.Start();
var funnyTask = new SubRedditTask("funny", redditHttpClient, subRedditManagerService);
funnyTask.Start();

app.Run();


