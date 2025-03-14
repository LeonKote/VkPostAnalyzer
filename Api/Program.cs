using Domain.Interfaces.ApiClients;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.ApiClients;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
	configuration
		.ReadFrom.Configuration(context.Configuration)
		.ReadFrom.Services(services);
});

builder.Services.Configure<VkApiOptions>(builder.Configuration.GetSection("Vk"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});
builder.Services.AddHttpClient<IVkApiClient, VkApiClient>();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("VkPostAnalyzerDb")));

builder.Services.AddScoped<IVkAuthService, VkAuthService>();
builder.Services.AddScoped<IVkPostAnalyzerService, VkPostAnalyzerService>();
builder.Services.AddScoped<IAuthRequestRepository, AuthRequestRepository>();
builder.Services.AddScoped<ILetterCountRepository, LetterCountRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
