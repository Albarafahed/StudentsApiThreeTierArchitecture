var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(optitions =>
 optitions.AddPolicy("StudentApiCorsPolicy", policy =>
 policy.WithOrigins("https://localhost:7222", "http://localhost:5155")

       .AllowAnyHeader()
       .AllowAnyMethod()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("StudentApiCorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
