using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
    // config.ApiVersionReader = new QueryStringApiVersionReader();
    // config.ApiVersionReader = new HeaderApiVersionReader();

    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"));
});

// Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        "api.xml"));

    options.SwaggerDoc("v1",
        new OpenApiInfo() { Title = "Cities Web API", Version = "1.0" });

    options.SwaggerDoc("v2",
        new OpenApiInfo() { Title = "Cities Web API", Version = "2.0" });
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// CORS: localhost:4200
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration
                .GetSection("AllowedOrigins").Get<string[]>())
            .WithHeaders("Authorization", "origin", "accept",
                "content-type")
            .WithMethods("GET", "POST", "PUT", "DELETE");
    });

    options.AddPolicy("4100Client", policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration
                .GetSection("AllowedOrigins2").Get<string[]>())
            .WithHeaders("Authorization", "origin", "accept")
            .WithMethods("GET");
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});

app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
