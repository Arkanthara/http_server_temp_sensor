using littlemichelserver.Controllers;
using littlemichelserver;
using System.Data.SQLite;
using littlemichelserver.TemperatureSensorDB;

SQLDB DataBase = new SQLDB();
DataBase.OpenDataBase("DataBase");
DataBase.CreateTable();

WebApplicationBuilder builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();