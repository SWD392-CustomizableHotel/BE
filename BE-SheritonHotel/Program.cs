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
using Microsoft.AspNetCore.Http.Features;
using SWD.SheritonHotel.Domain.Handlers;
using Stripe;
using FluentValidation;
using FluentValidation.AspNetCore;
using SWD.SheritonHotel.Validator;
using System.Reflection;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.Text;
using BookingService = Entities.BookingService;
using SWD.SheritonHotel.Validator.Interface;
using SWD.SheritonHotel.Domain.Configs.Firebase;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using SWD.SheritonHotel.API.WebSocket;
using SWD.SheritonHotel.Domain.Commands;
using Google.Api;


var builder = WebApplication.CreateBuilder(args);

//Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\VisionAI\vision-ai-428311-e8ec8670484d.json");

builder.Services.AddControllers();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 20971520; // 20MB
});
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type =>
    {
        if (type == typeof(CreatePaymentIntentCommand.Item))
            return "CreatePaymentIntentCommand_Item";
        if (type == typeof(CreatePaymentIntentCustomizableCommand.Item))
            return "CreatePaymentIntentCustomizableCommand_Item";
        return type.FullName;
    });
});
StripeConfiguration.ApiKey = "sk_test_51PZTGERt4Jb0KcASvnNu77y3c6lmQJNpLD3gvERz0vPLhPNERogsVubVaRuUb2xNYC6o4r0ZZ7ZH3eXh1jd715Ft00eh5S5EDO";


#region Add Dbcontext
// Add DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SheritonDB_D");
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

// Config FPT AI
builder.Services.Configure<FPTAIOptions>(builder.Configuration.GetSection("FPTAI"));
builder.Services.AddHttpClient();
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

builder.Services.Configure<JwtBearerOptions>(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"])),
    };
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
    options.OperationFilter<FileUploadOperationFilter>();

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
builder.Services.AddMediatR(typeof(GetAllServicesQueryHandler).Assembly);
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAmentiyRepository, AmenityRepository>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IBookingAmenityRepository, BookingAmenityRepository>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IManageService, ManageServiceService>();
builder.Services.AddScoped<ITokenValidator, TokenValidator>();
builder.Services.AddScoped<IIdentityCardRepository, IdentityCardRepository>();
builder.Services.AddScoped<IIdentityCardService, IdentityCardService>();
builder.Services.AddScoped<IAccountService, SWD.SheritonHotel.Services.Services.AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAssignServiceService, AssignServiceService>();
builder.Services.AddScoped<IAssignServiceRepository, AssignServiceRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepostitory>();
builder.Services.AddScoped<IBookingService, BookingHistoryService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<EmailVerify>();
builder.Services.AddScoped<IPaymentIntentCustomizeService, PaymentIntentCustomizeService>();
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddSingleton<SocketIOServer>();
builder.Services.AddHostedService<ApplicationWorker>();
#endregion

#region Add MediatR

var handler = typeof(AppHandler).GetTypeInfo().Assembly;
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), handler);
builder.Services.AddMediatR(typeof(UpdateUserCommandHandler).Assembly);
builder.Services.AddMediatR(typeof(CreatePaymentForLaterHandler).Assembly);

builder.Services.AddTransient<IRequestHandler<CreatePaymentIntentCommand, List<string>>, CreatePaymentIntentHandler>();
builder.Services.AddTransient<IRequestHandler<CreatePaymentIntentCustomizableCommand, List<string>>, CreatePaymentIntentCustomizeCommandHandler>();

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

#region FluentValidator
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateServiceCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateServiceCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UploadIdentityCardCommandValidator>();
#endregion
// Add Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.MaxDepth = 32;
});
#region FireBase
builder.Services.Configure<FirebaseConfig>(builder.Configuration.GetSection("FirebaseConfig"));
// Set the environment variable for Google credentials
var firebaseConfig = builder.Configuration.GetSection("FirebaseConfig").Get<FirebaseConfig>();
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(builder.Environment.ContentRootPath, firebaseConfig.KeyFilePath));
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
await app.RunAsync();