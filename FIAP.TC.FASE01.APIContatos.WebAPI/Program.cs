using System.Data;
using System.Reflection;
using Asp.Versioning;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using FIAP.TC.FASE01.APIContatos.Infrastrucure.Repositories;
using MediatR;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
// builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(Assembly.Load("FIAP.TC.FASE01.APIContatos.Application"));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

