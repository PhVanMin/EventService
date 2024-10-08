using EventService.API.Extensions;
using EventService.API.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.ConfigurateLogging();
builder.AddAuthenticationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
//app.UseExceptionHandler(opt => { });

// TODO: maybe add versioning idk?
app.MapControllers();
app.MapGrpcService<GreeterService>();

app.Run();