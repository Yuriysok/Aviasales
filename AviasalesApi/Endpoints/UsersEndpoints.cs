using AviasalesApi.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AviasalesApi.Endpoints
{
    public static class UsersEndpoints
    {
        public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            var userGroup = app.MapGroup("api/users");
            userGroup.RequireAuthorization("user");

            userGroup.MapGet("", GetUsers);
            //userGroup.MapGet("{id}", GetUser);
            //userGroup.MapPut("", PutUser);
            //userGroup.MapDelete("{id}", DeleteUser);

            var authGroup = app.MapGroup("auth");
            authGroup.MapPut("register", Register);
            authGroup.MapPost("login", Login);
        }

        private static async Task<Results<Ok, BadRequest>> Register(DataContext context, UserDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var newUser = new User 
            { 
                Name = userDto.Name,
                PasswordHash = passwordHash
            };
            try
            {
                context.Users.Add(newUser);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TypedResults.BadRequest("Name is not unique");
            }
            return TypedResults.Ok();
        }

        private static async Task<Results<Ok<string>, BadRequest<string>>> Login(DataContext context, UserDto userDto)
        {
            var badRequestError = TypedResults.BadRequest("Wrong login or password");

            var user = await context.Users.SingleOrDefaultAsync(user => user.Name == userDto.Name);
            if (user == null)
                return badRequestError;

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash)) {
                return badRequestError;
            }

            var token = CreateToken(user);

            return TypedResults.Ok(token);
        }

        private static string CreateToken(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name)};

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static async Task<Ok<List<User>>> GetUsers(DataContext context)
        {
            return TypedResults.Ok(await context.Users.ToListAsync());
        }

        private static async Task<Results<Ok<User>, NotFound<string>>> GetUser(DataContext context, int id)
        {
            var result = await context.Users.FindAsync(id);
            return result != null
                ? TypedResults.Ok(result)
                : TypedResults.NotFound($"User with id = {id} not found");
        }

        private static async Task<IResult> PutUser(DataContext context, User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Results.Ok();
        }

        private static async Task<IResult> DeleteUser(DataContext context, int id)
        {
            context.Users.Where(user => user.Id == id).ExecuteDelete();
            await context.SaveChangesAsync();
            return Results.Ok();
        }
    }
}
