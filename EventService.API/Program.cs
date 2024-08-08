using EventService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.AddApplicationServices();
builder.AddLoggingServices();
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
    }
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// TODO: maybe add versioning idk?
app.MapControllers();

app.Run();