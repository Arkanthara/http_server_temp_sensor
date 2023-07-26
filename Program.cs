using littlemichelserver.Controllers;
using littlemichelserver;
using System.Data.SQLite;
using littlemichelserver.TemperatureSensorDB;

SQLDB DataBase = new SQLDB();
DataBase.OpenDataBase("DataBase");
DataBase.CreateTable();

WebApplicationBuilder builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  // It's required for the generation of Swagger for Minimal APIs (https://blog.devgenius.io/)
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();   // I don't know why it's here...

app.UseAuthorization();  // I don't know why it's here...

app.MapControllers();

app.Run();