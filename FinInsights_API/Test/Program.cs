using System;
using Test.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Extensions;
using AutoMapper;




var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});
builder.Services.AddScoped<JwtService>();
builder.Services.AddDbContext<RaghuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IEmailClient, EmailClient>();
builder.Services.AddHttpClient<IMfService, MfService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddAutoMapper(typeof(NewsMappingProfile));// {
//     options.AddPolicy("AllowAll",
//         policy => policy.WithOrigins("http://localhost:4200") 
// .AllowCredentials()
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
// });
builder.Services.AddTransient<ServiceTokenHandler>();
builder.Services.AddHttpClient("InternalServices")
    .AddHttpMessageHandler<ServiceTokenHandler>();
builder.Services.AddScoped<IServiceInvoker, ServiceInvoker>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.WithOrigins("AllowAll")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.Configure<OpenRouterSettings>(
    builder.Configuration.GetSection("OpenAI")
);
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
//Api Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");   // ✅ FIRST apply CORS

//app.UseMiddleware<JwtMiddleware>(); // 👈 AFTER CORS
app.UseHttpsRedirection();
app.UseAuthentication();   // 🔥 MUST come before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
