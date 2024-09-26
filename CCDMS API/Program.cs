using CCDMS_API.Extensions;
using CCDMSServices.ORM.Context;
using CCDMSServices.ORM.MigrationRunner;
using CCDMSServices.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabaseService(builder.Configuration);

builder.Services.AddScoped<ICCDMSService,CCDMSService>();
builder.Services.AddScoped<IMigrationRunner,MigrationRunner>();

var app = builder.Build(); 
app.ApplyPendingMigrations();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
