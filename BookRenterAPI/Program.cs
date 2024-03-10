using BookRenterData.Context;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterData.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BookRenter.Services.Interfaces;
using BookRenter.Services;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using BookRenterService.Concrete;


var builder = WebApplication.CreateBuilder(args);
// Add your services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Assuming UnitOfWork implements IUnitOfWork
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserClaimService, UserClaimService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<ICheckoutService, CheckoutService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICartService, CartService>();

//builder.Services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
//builder.Services.AddSingleton(Configuration.GetSection("JwtSettings").Get<JwtSettings>());

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Set to true if you want to validate the issuer
            ValidateAudience = false, // Set to true if you want to validate the audience
            ValidateLifetime = true, // Validate token expiration
            ValidateIssuerSigningKey = true, // Validate the signing key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Configure JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Configure JWT Bearer token authentication
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


// Add DbContext and specify connection string
builder.Services.AddDbContext<BookRenterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookRenterDatabase")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1"));

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();
