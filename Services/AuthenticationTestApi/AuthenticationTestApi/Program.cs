using Cleave.Middleware.Authentication;
using Cleave.Middleware.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCleaveExceptionHandler();
builder.Services.AddCleaveAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCleaveExceptionHandler();
app.UseCleaveAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
