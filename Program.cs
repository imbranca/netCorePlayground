using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApp.Namespace;
using RestApiScratch.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 8;
})
.AddRoleManager<RoleManager<IdentityRole<int>>>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
      {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }
      // {
      //   options.DefaultScheme = "Cookies"; // Or another default scheme
      //   options.DefaultChallengeScheme = "oidc"; // Or your external provider scheme
      // }
      )
      .AddJwtBearer(options => {
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddCookie("External")
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        // googleOptions.SaveTokens = true;
        googleOptions.CallbackPath = "/signin-google";

        // googleOptions.SignInScheme = "External";
        googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
        // googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });



// builder.Services.AddAuthentication().AddGoogle(googleOptions =>
// {
//     googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//     googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
// });

builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddTransient<ApplicationSeeder>();
// builder.Services.AddTransient<ISeeder, DataSeeder>();
// builder.Services.AddTransient<ISeeder, ProductSeeder>();
// builder.Services.AddTransient<ISeeder, UsersSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  //Migrate
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  db.Database.Migrate();

  //Excecute seeders
  var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
  await seeder.SeedAllAsync(db, scope.ServiceProvider);
}

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

app.MapGet("/login/google", () =>
{
       var props = new AuthenticationProperties
    {
        RedirectUri = "/api/auth/success"
    };
  //We start the google auth here, any external reference here bc thats used on google callback
    return Results.Challenge(props, new[] { GoogleDefaults.AuthenticationScheme });
});

// app.MapGet("/api/auth/success", async (
//     HttpContext context) =>
// {
//     var result = await context.AuthenticateAsync("External");

//     if (!result.Succeeded)
//     {
//         return Results.Unauthorized();
//     }
//         var user = result.Principal;
//     Console.WriteLine("PRINCIPAL ",result.Principal);
//     Console.WriteLine("PRINCIPAL: " + user);

//     // Example: read Google claims
//     var email = user.FindFirst(ClaimTypes.Email)?.Value;
//     var name = user.FindFirst(ClaimTypes.Name)?.Value;

//     Console.WriteLine("EMAIL: " + email);
//     Console.WriteLine("NAME: " + name);
//     // await accountService.LoginWithGoogleAsync(result.Principal);

//     return Results.Ok();

// }).WithName("GoogleLoginCallback");

app.Run();