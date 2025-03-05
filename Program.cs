using Microsoft.EntityFrameworkCore;
using Serilog;
using VkPostAnalyzer.Domain.Interfaces;
using VkPostAnalyzer.Infrastructure.ApiClients;
using VkPostAnalyzer.Infrastructure.Data;
using VkPostAnalyzer.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});
builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("VkPostAnalyzerDb")));

builder.Services.AddScoped<IVkApiClient, VkApiClient>();
builder.Services.AddScoped<IVkAuthService, VkAuthService>();
builder.Services.AddScoped<IVkPostAnalyzerService, VkPostAnalyzerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
