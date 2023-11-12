using Microsoft.EntityFrameworkCore;
using RobotCleaner.Api;
using RobotCleaner.Api.Features.Clean;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RobotCleaner.RobotCleaner>();
builder.Services.AddScoped<IClean, Clean>();
builder.Services.AddScoped<ISaveCommandsRepository, CleanRepository>();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddDbContext<CleanContext>(
    ee => { ee.UseNpgsql(builder.Configuration.GetConnectionString("RobotCleaner")); });

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.UseCustomEfMigration();

app.Run();