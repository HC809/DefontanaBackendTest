using DefontanaBackendTest.BLL.Services;
using DefontanaBackendTest.BLL.Services.Interfaces;
using DefontanaBackendTest.DAL;
using DefontanaBackendTest.DL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IConsultasService, ConsultasService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Defontana BE Test / Hector Caballero", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "api-docs/{documentName}/swagger.json"; // Custom route for the JSON endpoint
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api-docs/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "My API Documentation";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
