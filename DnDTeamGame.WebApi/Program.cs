using DnDTeamGame.Data;
using DnDTeamGame.Data.Entities;
using DnDTeamGame.Services.UserServices;
using DnDTeamGame.Services.TokenServices;
using DnDTeamGame.Services.CharacterServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DnDTeamGame.Services.Games;
using DnDTeamGame.Services.MapServices;
using DnDTeamGame.Services.VehicleServices;
using DnDTeamGame.Services.WeaponServices;
using DnDTeamGame.Services.AbilityServices;
using DnDTeamGame.Services.ArmourServices;
using DnDTeamGame.Services.ConsumableServices;
using DnDTeamGame.Services.CharacterClassServices;
using DnDTeamGame.Services.HairColorServices;
using DnDTeamGame.Services.HairStyleServices;
using DnDTeamGame.Services.BodyTypeServices;
using DnDTeamGame.Models.MapProfile;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")
            )
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("https://dndwebapi.azurewebsites.net", "http://127.0.0.1:5500")
                               .AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<UserEntity>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;

})
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IWeaponService, WeaponService>();
builder.Services.AddScoped<IMapGenerator, MapGenerator>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IAbilityService, AbilityService>();
builder.Services.AddScoped<IArmourService, ArmourService>();
builder.Services.AddScoped<IConsumableService, ConsumableService>();
builder.Services.AddScoped<IHairStyleService, HairStyleService>();
builder.Services.AddScoped<IHairColorService, HairColorService>();
builder.Services.AddScoped<ICharacterClassService, CharacterClassService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization Header using the Bearer scheme.\n\n"
        + "Enter 'Bearer' and then your token in the text input below.\n\n"
        + "Example: \"Bearer 12345abcdef\""
    });

    c.AddSecurityRequirement(new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[]{}
        }
    });
});



builder.Services.AddAutoMapper(typeof(AbilityAutoMapProfile));
builder.Services.AddAutoMapper(typeof(ArmourAutoMapProfile));
builder.Services.AddAutoMapper(typeof(CharacterClassAutoMapProfile));
builder.Services.AddAutoMapper(typeof(ConsumableAutoMapProfile));
builder.Services.AddAutoMapper(typeof(HairStyleAutoMapProfile));
builder.Services.AddAutoMapper(typeof(HairColorAutoMapProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
