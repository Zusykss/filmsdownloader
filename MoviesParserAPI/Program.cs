using Core.Helpers;
using Core.Helpers.Options;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomServices();
builder.Services.Configure<ParserSettings>(builder.Configuration.GetSection(nameof(ParserSettings)));
builder.Services.AddUnitOfWork();

builder.Services.AddRepository();

builder.Services.AddAutoMapper();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options =>
    options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
