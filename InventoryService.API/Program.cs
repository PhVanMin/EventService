using InventoryService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<InventoryDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("database")
));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin() 
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseExceptionHandler(opt => { });
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
