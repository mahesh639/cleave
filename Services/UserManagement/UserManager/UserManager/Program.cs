using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManager.Data;
using UserManager.Models;
using UserManager.Repository;
using Cleave.Middleware.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

//Register Cors
builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
{
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddMvc();

//Add Db Context
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevIdentityConnectionString") ?? throw new InvalidOperationException())
);

//AddIdentity
builder.Services.AddIdentity<UserDetails, IdentityRole>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddSignInManager()
        .AddRoles<IdentityRole>();

//Add JWT Authentication
builder.Services.AddAuthentication(options => { 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.AddScoped<IAccountRespository, AccountRepository>();
builder.Services.AddTransient<CleaveExceptionHandler>();

//This dependency is added for Development purpose only.
builder.Services.AddScoped<IPasswordHasher<UserDetails>, CustomPasswordHasher>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//use cors
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCleaveExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//This is for Development use only
public class CustomPasswordHasher : IPasswordHasher<UserDetails>
{
    public string HashPassword(UserDetails user, string password)
    {
        return password;
    }

    public PasswordVerificationResult VerifyHashedPassword(UserDetails user, string hashedPassword, string providedPassword)
    {
        return hashedPassword.Equals(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
    }
}
