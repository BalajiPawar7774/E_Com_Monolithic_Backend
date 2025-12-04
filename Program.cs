using E_Com_Monolithic.Authentication.AuthRepositories;
using E_Com_Monolithic.Authentication.Jwt;
using E_Com_Monolithic.Dal;
using E_Com_Monolithic.Models;
using E_Com_Monolithic.Repositories;
using E_Com_Monolithic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>( options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//Configure jwt acuthentication from Cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(options =>
  {
      //configure token validation parameters
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = "E-Com-Monolithic",

          ValidateAudience = true,
          ValidAudience = "E-Com-Front-End",

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["jwt:SecretKey"])),

          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
      };

      // Configure how JWT token is extracted from request
      options.Events = new JwtBearerEvents
      {
          
          OnMessageReceived = context =>
          {
              if (context.Request.Cookies.ContainsKey("auth_token"))
              {
                  var iftoken = context.Request.Cookies["auth_token"];
                  Console.WriteLine($"Token from cookie: {iftoken?.Substring(0, Math.Min(20, iftoken.Length))}...");
                  context.Token = iftoken;
              }
              else
              {
                  Console.WriteLine("No auth_token cookie found!");
              }

              var token = context.Request.Cookies["auth_token"];

              if (!string.IsNullOrEmpty(token))
              {
                  context.Token = token;
              }

              else if (context.Request.Headers.ContainsKey("Authorization"))
              {
                  var authHeader = context.Request.Headers["Authorization"].ToString();
                  if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                  {
                      context.Token = authHeader.Substring("Bearer ".Length).Trim();
                  }
              }

              return Task.CompletedTask;
          },

          // Optional: Handle authentication failures
          OnAuthenticationFailed = context =>
          {
              Console.WriteLine($"Authentication failed: {context.Exception.Message}");
              return Task.CompletedTask;
          }
      };
  });




builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<ICommonRepository<Product>, CommonRepository<Product>>();
builder.Services.AddTransient<ICommonRepository<Category>, CommonRepository<Category>>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddTransient<ProductService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
      policy.WithOrigins("http://localhost:4200", "http://localhost:4200/")  // list exact origins
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

builder.Services.AddControllers();
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

app.UseCors("MyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
