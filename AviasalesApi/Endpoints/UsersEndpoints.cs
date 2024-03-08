using AviasalesApi.Models;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AviasalesApi.Endpoints
{
    public class UsersEndpoints(IConfiguration config) : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var userGroup = app.MapGroup("api/users");

            userGroup.MapGet("", GetUsers).AllowAnonymous();
            //userGroup.MapGet("{id}", GetUser);
            //userGroup.MapPut("", PutUser);
            //userGroup.MapDelete("{id}", DeleteUser);

            var authGroup = app.MapGroup("auth");
            authGroup.AllowAnonymous();
            authGroup.MapPut("register", Register);
            authGroup.MapPost("login", Login);
        }

        private async Task<Results<Ok, BadRequest<string>>> Register(DataContext context, UserDto userDto)
        {
            const int DuplicateInsertErrorNumber = 2601;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var newUser = new User
            {
                Name = userDto.Name,
                PasswordHash = passwordHash
            };
            ;
            context.Users.Add(newUser);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (((SqlException)ex.InnerException!).Number == DuplicateInsertErrorNumber)
                    return TypedResults.BadRequest($"Name \"{userDto.Name}\" is not unique");
                throw;
            }
            return TypedResults.Ok();
        }

        private async Task<Results<Ok<string>, BadRequest<string>>> Login(DataContext context, UserDto userDto)
        {
            var badRequestError = TypedResults.BadRequest("Wrong login or password");

            var user = await context.Users.SingleOrDefaultAsync(user => user.Name == userDto.Name);
            if (user == null)
                return badRequestError;

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return badRequestError;
            }

            var token = CreateToken(user);

            return TypedResults.Ok(token);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, user.Name)
            };

            var jwt = config.GetValue<string>("Jwt")!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Ok<List<User>>> GetUsers(DataContext context, IDistributedCache cache, CancellationToken ct)
        {
            var users = await cache.GetAsync("users",
                async token =>
                {
                    var users = await context.Users.ToListAsync(token);
                    return users;
                }, Options.CacheOptions.DefaultExpiration, ct);

            return TypedResults.Ok(users);
        }

        private async Task<Results<Ok<User>, NotFound<string>>> GetUser(DataContext context, int id)
        {
            var result = await context.Users.FindAsync(id);
            return result != null
                ? TypedResults.Ok(result)
                : TypedResults.NotFound($"User with id = {id} not found");
        }

        private async Task<IResult> PutUser(DataContext context, User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Results.Ok();
        }

        private async Task<IResult> DeleteUser(DataContext context, int id)
        {
            context.Users.Where(user => user.Id == id).ExecuteDelete();
            await context.SaveChangesAsync();
            return Results.Ok();
        }
    }
}
