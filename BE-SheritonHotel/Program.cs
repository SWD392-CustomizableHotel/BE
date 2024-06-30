using Entities;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Services;
using SWD.SheritonHotel.Data.Repositories;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Utilities;
using SWD.SheritonHotel.Handlers;
using SWD.SheritonHotel.Handlers.Handlers;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Services;
using SWD.SheritonHotel.Services.Services;
using SWD.SheritonHotel.Data.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using SWD.SheritonHotel.Validator;
using System.Reflection;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Add Dbcontext
// Add DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
});
#endregion

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

#region Add, Config Identity and Role
// Add Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
});

// Config Token expiration
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromDays(1));

#endregion

#region JwtBear and Authentication, Swagger API

// Add Authentication and JwtBearer
var jwtSettings = builder.Configuration.GetSection("JWT");

builder.Services
    .AddAuthentication(options =>
    {
        // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["ValidIssuer"],
            ValidAudience = jwtSettings["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"])),
        };
    })
    .AddGoogle(googleOptions =>
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("GoogleAuthSettings:Google");
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter your token with this format: ''Bearer YOUR_TOKEN''",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    options.MapType<ServiceStatus>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(ServiceStatus))
                    .Select(name => (IOpenApiAny)new OpenApiString(name)).ToList()
    });
    options.MapType<AmenityStatus>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(AmenityStatus))
                   .Select(name => (IOpenApiAny)new OpenApiString(name)).ToList()
    });
});


#endregion

#region Add Scoped
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IViewRoomRepository, ViewRoomRepository>();
builder.Services.AddScoped<IViewRoomService, ViewRoomService>();
builder.Services.AddMediatR(typeof(GetAllAvailableRoomQueryHandler).Assembly);
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAmentiyRepository, AmenityRepository>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IManageService, ManageServiceService>();
#endregion

#region Add MediatR

var handler = typeof(AppHandler).GetTypeInfo().Assembly;
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), handler);

builder.Services.AddScoped<EmailVerify>();
builder.Services.AddScoped<TokenGenerator>();
#endregion

#region Add CORS
//CORS
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("https://fe-customizablehotel.vercel.app", "http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
}));
#endregion

#region Mapping Profile

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

#region Mapping Profile

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

#region FluentValidator
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateServiceCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateServiceCommandValidator>();
#endregion


// #region Add MediateR
// var handler = typeof(GetAllRoomsQueryHandler).GetTypeInfo().Assembly;
// builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), handler);
// #endregion
// pipeline

//#region Add JsonNaming
//builder.Services.AddControllers().AddJsonOptions(options => {
//    options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
//});
//#endregion

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//RUN
app.Run();