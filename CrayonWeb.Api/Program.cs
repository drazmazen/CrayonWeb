using CrayonWeb.Api.CCP;
using CrayonWeb.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connString = builder.Configuration.GetConnectionString("CrayonDb");
builder.Services.AddDbContext<CrayonDbContext>(option => option.UseSqlServer(connString));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICcpClient, CcpClientDev>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //if we are in the dev environment, migrate db and populate some data
    DevDb.Populate(app);
}
else
{
    builder.Services.AddScoped<ICcpClient, CcpClientProd>();
}

app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());   

app.Run();
