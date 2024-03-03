using AviasalesApi;
using AviasalesApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
  .AddPolicy("user", policy =>
        policy
            .RequireRole("user")
            .RequireClaim("claimType", "claimValue"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapUsersEndpoints();

app.UseHttpsRedirection();

app.Run();
