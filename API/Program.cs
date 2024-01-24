using Domain;
using Infrastructure.Data;
using Infrastructure.Dependency;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Dependency;
using System.Configuration;
using Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var configuration = builder.Configuration;


builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddIdentityService(configuration);

builder.Services.AddInfrastructureService(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();;
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

	 await dbInitializer.Initialize();
}
app.Run();
