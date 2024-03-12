using AviasalesApi;
using AviasalesApi.Extensions;
using AviasalesApi.Services;
using Carter;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication().AddJwtBearer();
//builder.Services.AddAuthorizationBuilder()
//  .AddFallbackPolicy("UserPolicy", policy =>
//    policy.RequireClaim(ClaimTypes.Role, "User"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDbContext<DataContext>();
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));
builder.Services.AddTransient<IAirlineService, AirlineService>();
builder.Services.AddCarter();
builder.Services.AddAdapters();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapCarter();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();
