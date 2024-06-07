using capygram.Auth.DependencyInjection.Extensions;
using capygram.Auth.Domain.Data;
using capygram.Auth.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddConfigOption(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var user = scope.ServiceProvider.GetService<IUserContext>().Users;
    var encypter = scope.ServiceProvider.GetService<IEncrypter>();
    await Seeder.Seed(user, encypter);
}
app.Run();
