
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/worker-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

// Register application services
builder.Services.AddScoped<Order.Application.Interfaces.IRabbitMqPublisher, Order.Infrastructure.Messaging.RabbitMqPublisher>();
builder.Services.AddScoped<Order.Application.Interfaces.ICsvUploadService, Order.Application.Services.CsvUploadService>();


var app = builder.Build();

// เปลี่ยนจาก
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// เป็น
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
