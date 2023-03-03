using DogApp;
using DogApp.Data;
using DogApp.Interface;
using DogApp.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
  x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<Seed>(); //this is nice to have if you don't have any data to play with for testing purposes (prior to post/put methods)
builder.Services.AddScoped<IPersonRepository, PersonRepository>();       //NEED this so the controller can see the interface/repository
builder.Services.AddScoped<IDogRepository, DogRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //NEED this if using AutoMapper (maps fields to SQL data records automatically)

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //little builder function to connect to my specific server
});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
  SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

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
