using GitHubApi.CacheServices;
using GitHubPortfolio.Service;
using GithubService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// טוען את קובץ ההגדרות
builder.Services.Configure<GitHubOption>(builder.Configuration.GetSection("GitHub"));

// רישום שירות ה־GitHub עם קונפיגורציה
builder.Services.AddGitHub(options => builder.Configuration.GetSection("GitHub").Bind(options));

// רישום זיכרון קאש
builder.Services.AddMemoryCache();

// אם אתה רוצה לעטוף את השירות
builder.Services.Decorate<IGitHubService, CacheGitHubServices>();

// הדפסת כל השירותים שנרשמו
foreach (var service in builder.Services)
{
    Console.WriteLine($"Service: {service.ServiceType}, Implementation: {service.ImplementationType}");
}

// אחרי זה נבנה את האפליקציה
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
