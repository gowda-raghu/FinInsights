using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class JWTMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public JWTMiddleware(
        RequestDelegate next,
        IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task Invoke(HttpContext context)
    {
        var authHeader =
            context.Request.Headers["Authorization"]
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing token");

            return;
        }

        var token = authHeader.Replace("Bearer ", "");
        Console.WriteLine(token);

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                _config["Jwt:Key"]))
                },
                out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine("================================");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("================================");

            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(ex.ToString());

            return;
        }

        await _next(context);
    }
}