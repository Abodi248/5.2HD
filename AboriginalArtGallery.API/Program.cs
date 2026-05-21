using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Services registered here
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Aboriginal Art Gallery API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aboriginal Art Gallery API v1"));

app.UseAuthorization();
app.MapControllers();

app.MapGet("/api/health", () => "ok");

app.Run();
