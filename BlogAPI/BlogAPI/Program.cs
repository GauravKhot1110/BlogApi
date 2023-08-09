using BlogAPI.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
var connectionstring = builder.Configuration.GetConnectionString("DbConnection");
var allowedHost = builder.Configuration.GetValue<string>("AllowedOrigins");
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(cors => cors.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<JWTHelper>();
builder.Services.AddScoped<DataContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id= "Bearer"
                        }
                    },
                    new string[]{}
                    }
                });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            string ErrorMessage;
            if (context.Error != null) ErrorMessage = context.Error + ". " + context.ErrorDescription;
            else ErrorMessage = "Token cannot be null";

            context.HandleResponse();
            var exception = new UnauthorizedAccessException(ErrorMessage);
            //var errorResponse = ExceptionFactoryClass.ExceptionResponse(exception, context.HttpContext);
            var errorResponse = "";
            var result = JsonSerializer.Serialize(errorResponse);
            logger.LogError(exception, exception.Message);
            await context.Response.WriteAsync(result);
        }
    };
});
var app = builder.Build();
//app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.ConfigureCustomExceptionMiddleware();
app.UseCors("MyPolicy");
app.UseRouting();
app.UseCors(builder =>
 builder.WithOrigins("http://localhost:4200")
 .AllowAnyHeader()
 .AllowCredentials()
 .AllowAnyMethod());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
builder.Services.AddDbContext<DataContext>(option =>
option.UseInMemoryDatabase("WebApiDatabase"));

 
