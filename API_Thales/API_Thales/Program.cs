using BusinessLayer;
using DataAccess;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Agregar HttpClient para consumir las APIs
builder.Services.AddHttpClient<EmployeeService>();

// Registrar EmployeeService como servicio
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<EmployeeManager>();
// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin() // Permite cualquier origen
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Aplica la política CORS
app.UseCors("AllowAllOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
