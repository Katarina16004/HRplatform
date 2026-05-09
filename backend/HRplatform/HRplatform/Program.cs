using HRplatform.Application.Interfaces;
using HRplatform.Application.Services;
using HRplatform.Infrastructure.Db;
using HRplatform.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// added DB connection string from configuration (appsettings.json and User Secrets)
var cs = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(cs))
{
    throw new InvalidOperationException(
        "Missing ConnectionStrings:Default. Set it in User Secrets.");
}

// for using DI 
// once for app lifetime
builder.Services.AddSingleton(new MySqlConnectionFactory(cs));
// once for request
builder.Services.AddScoped<ISkillRepository, SkillRepository>(); 
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();    
builder.Services.AddScoped<CandidateService>();

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