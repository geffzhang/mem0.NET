using mem0.NET.Service.DataAccess;
using mem0.NET.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace mem0.NET.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMem0DotNetEntityFramework(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMem0DotNet(options =>
        {
            options.OpenAIEndpoint = "https://api.token-ai.cn";
            options.OpenAIKey = "请在 https://api.token-ai.cn 注册获取";
            options.OpenAIChatCompletionModel = "gpt-4o-mini";
            options.OpenAITextEmbeddingModel = "text-embedding-ada-002";
            options.CollectionName = "mem0.NET";
        }).AddVectorQdrant(options =>
        {
            options.Host = "localhost";
            options.Port = 6334;
            options.Https = false;
            options.ApiKey = "dd666666";
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<Mem0DbContext>();

            dbContext.Database.EnsureCreatedAsync();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapMemoryService();

        app.Run();
    }
}