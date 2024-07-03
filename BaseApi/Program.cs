using CoreDomain;
using CoreUtilities.DI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var domainAssembly = Assembly.GetAssembly(typeof(BaseModel));
if (domainAssembly == null)
{
    throw new Exception("Could not find domain assembly");
}

builder.Services.BuildDependencyInjectionFromAttribute(domainAssembly);
var app = builder.Build();

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
