using GoVilla.API.Endpoints.Apartments;
using GoVilla.API.Endpoints.Bookings;
using GoVilla.API.Endpoints.Reviews;
using GoVilla.API.Endpoints.Users;
using GoVilla.API.Extensions;
using GoVilla.Application;
using GoVilla.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    // app.SeedData();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapApartmentsEndpoints();
app.MapBookingsEndpoints();
app.MapReviewsEndpoints();
app.MapUsersEndpoints();

app.Run();