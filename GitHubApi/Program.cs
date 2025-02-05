using GitHubApi.CacheServices;
using GitHubPortfolio.Service;
using GithubService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GitHubOption>(builder.Configuration.GetSection("GitHub"));
builder.Services.AddGitHub(options => builder.Configuration.GetSection("GitHub").Bind(options));
builder.Services.AddMemoryCache();
builder.Services.Decorate<IGitHubService, CacheGitHubServices>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
