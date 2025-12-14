using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using SecondBrain.Infrastructure.Db;
using SecondBrain.Api.Config;
using SecondBrain.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<SecondBrainDbContext>(options =>
    options.UseNpgsql(
        "Host=localhost;Port=5432;Database=secondbrain;Username=postgres;Password=postgres",
        npgsqlOptions =>
        {
            npgsqlOptions.UseVector();
        }));

builder.Services.Configure<OpenAiOptions>(
    builder.Configuration.GetSection("OpenAI"));

var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];

if (string.IsNullOrWhiteSpace(openAiApiKey) ||
    openAiApiKey.StartsWith("YOUR_"))
{
    builder.Services.AddSingleton<IEmbeddingService, FakeEmbeddingService>();
}
else
{
    builder.Services.AddHttpClient<IEmbeddingService, OpenAiEmbeddingService>();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowFrontend");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
