using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RR.DiceDuel.Components;
using RR.DiceDuel.Core.Controllers.GameController;
using RR.DiceDuel.Core.Controllers.PlayerController;
using RR.DiceDuel.Core.Services.AuthService;
using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.Core.Services.SessionService;
using RR.DiceDuel.Core.StateMachine;
using RR.DiceDuel.Core.StateMachine.Interfaces;
using RR.DiceDuel.ExternalServices.EntityFramework;
using RR.DiceDuel.ExternalServices.SignalR;

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RR.DiceDuel.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Set here your JWT token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<GameContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddSignalR();
builder.Services.AddScoped<IStateMachine, StateMachine>();

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

builder.Services.AddScoped<IGameController, GameController>();
builder.Services.AddScoped<IPlayerController, PlayerController>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey("MySuperSecretKeyWithAtLeast32Characters!"u8.ToArray()),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("api/game"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAntiforgery();
app.UseAuthorization();
app.MapControllers();

app.MapHub<GameHub>("api/game");

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();