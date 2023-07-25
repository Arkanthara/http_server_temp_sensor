using littlemichelserver.Controllers;
using littlemichelserver;
using System.Data.SQLite;
using littlemichelserver.TemperatureSensorDB;
using static littlemichelserver.Controllers.TestController;

SQLDB DataBase = new SQLDB();
DataBase.OpenDataBase("DataBase");
DataBase.CreateTable();

WebApplicationBuilder builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  // I don't know why it's here...
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Data data = new Data();

Data.data_received = new Dictionary<string, string>();


app.UseHttpsRedirection();   // I don't know why it's here...

app.UseAuthorization();  // I don't know why it's here...

app.MapControllers();

app.Run();