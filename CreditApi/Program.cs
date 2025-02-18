using Credit.Api.Db;
using Credit.Api.Db.Repositories;
using Credit.Api.Endpoints;
using Dapper.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDapperBase>(sp => new CreditApiDb(builder.Configuration.GetConnectionString("CreditDb")));
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<ICreditRepository, CreditRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.RegisterCreditEnpoints();

using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await databaseInitializer.InitializeAsync();
}

app.Run();

//For integration testing
public partial class Program { }