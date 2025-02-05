using GitHubApi.CacheServices;
using GitHubPortfolio.Service;
using GithubService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ���� �� ���� �������
builder.Services.Configure<GitHubOption>(builder.Configuration.GetSection("GitHub"));

// ����� ����� ��GitHub �� �����������
builder.Services.AddGitHub(options => builder.Configuration.GetSection("GitHub").Bind(options));

// ����� ������ ���
builder.Services.AddMemoryCache();

// �� ��� ���� ����� �� ������
builder.Services.Decorate<IGitHubService, CacheGitHubServices>();

// ����� �� �������� ������
foreach (var service in builder.Services)
{
    Console.WriteLine($"Service: {service.ServiceType}, Implementation: {service.ImplementationType}");
}

// ���� �� ���� �� ���������
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
