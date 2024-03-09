using BookRenterData.Context;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterData.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BookRenter.Services.Interfaces;
using BookRenter.Services;

var builder = WebApplication.CreateBuilder(args);
// Add your services
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>(); // Assuming UnitOfWork implements IUnitOfWork
builder.Services.AddTransient<IBookService, BookService>();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext and specify connection string
builder.Services.AddDbContext<BookRenterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookRenterDatabase")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
