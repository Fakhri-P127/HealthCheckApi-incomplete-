using HealthCheck.DataService.Data;
using HealthCheck.DataService.IConfiguration;
using HealthCheck.Entities.Mapping;
using HealthCheck.Authentication.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using FluentValidation;
using HealthCheck.DataService.Validators.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(x => x.AddProfile(new GeneralMap()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(opt => opt
.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(opt =>
{
    //client e bizde olan butun api versiyalarini gosterir
    opt.ReportApiVersions = true;
    // bu avtomatik default versiya verecek
    opt.AssumeDefaultVersionWhenUnspecified = true;
    // default deyeri menimsedirik
    opt.DefaultApiVersion = ApiVersion.Default;
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestDtoValidator>();

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false, // Todo
    ValidateAudience = false,// Todo
    RequireExpirationTime = false,// Todo
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
//inject
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameters;

});

builder.Services.AddDefaultIdentity<IdentityUser>(opt => opt.SignIn.RequireConfirmedEmail = true)
    .AddEntityFrameworkStores<AppDbContext>();
var app = builder.Build();

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

app.Run();
