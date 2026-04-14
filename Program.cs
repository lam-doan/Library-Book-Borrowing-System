var builder = WebApplication.CreateBuilder(args);

// tell application we want to use controller
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// tell .net to auto set up api based on my controller
app.MapControllers();

app.Run();