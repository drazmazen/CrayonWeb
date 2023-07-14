using CrayonWeb.Api.CCP;
using CrayonWeb.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connString = builder.Configuration.GetConnectionString("CrayonDb");
builder.Services.AddDbContext<CrayonDbContext>(option => option.UseSqlServer(connString));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var useCcpClientProd = builder.Configuration.GetValue<bool>("Ccp:UseCcpClientProd");

if (useCcpClientProd)
{
    builder.Services.AddScoped<ICcpClient, CcpClientProd>();
}
else
{
    builder.Services.AddScoped<ICcpClient, CcpClientDev>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //if we are in the dev environment, migrate db and populate some data
    DevDb.Populate(app);
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());   

app.Run();
