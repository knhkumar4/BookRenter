using BookRenter.Services;
using BookRenter.Services.Interfaces;
using BookRenterAPI;
using BookRenterData.Context;
using BookRenterData.UnitOfWork;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Concrete;
using BookRenterService.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
ConfigureApp(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // DbContext and UnitOfWork
    services.AddDbContext<BookRenterContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("BookRenterDatabase")));
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // HTTP Context Accessor
    services.AddHttpContextAccessor();
    //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    //



    // Domain Services
    services.AddTransient<IUserClaimService, UserClaimService>();
    services.AddTransient<IBookService, BookService>();
    services.AddTransient<IInventoryService, InventoryService>();
    services.AddTransient<ICheckoutService, CheckoutService>();
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<ICartService, CartService>();

    // JWT Authentication
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Secret"]))
            };
        });

    // MVC Controllers
    services.AddControllers().AddFluentValidation();

    // Swagger
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookRenter API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}

void ConfigureApp(WebApplication app)
{
    app.UseExceptionMiddleware();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookRenter API v1"));
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}
