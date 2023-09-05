using Azure.Storage.Blobs;
using AzureBlobStorage.Api.Models;
using AzureBlobStorage.Api.Repository;
using AzureBlobStorage.Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton(u => 
    new BlobServiceClient(builder.Configuration.GetConnectionString("StorageConnection")));
builder.Services.AddTransient<IAzureBlobStorage, AzureApiBlobStorage>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.DescribeAllParametersInCamelCase();

    c.AddServer(new OpenApiServer
    {
        Description = "Development Server",
        Url = "http://localhost:5217"
    });
});

var app = builder.Build();

app.UseCors(x => x
    .WithOrigins("http://localhost:5217")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

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
