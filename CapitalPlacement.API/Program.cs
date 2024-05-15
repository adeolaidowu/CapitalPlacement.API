using CapitalPlacement.Core.IRepositories;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Services;
using CapitalPlacement.Infrastructure.Data;
using CapitalPlacement.Infrastructure.IRepositories;
using CapitalPlacement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
var databaseName = configuration["CosmosSettings:DatabaseName"];
builder.Services.AddDbContext<AppDbContext>(options => options.UseCosmos(configuration.GetConnectionString("DbConn"), databaseName));

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region repositories

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

#endregion

#region Services

builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

#endregion
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
