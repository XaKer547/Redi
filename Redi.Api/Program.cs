using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Redi.Api.Hubs;
using Redi.Api.Infrastructure;
using Redi.Api.Infrastructure.Interfaces;
using Redi.Application.Services;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities.Users;
using Redi.DataAccess.Data.Seeder;
using Redi.Domain.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

namespace Redi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(x => x.AddConsole());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("redi", new OpenApiInfo { Title = "Redi API" });
                c.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlFilePath);
            });

            builder.Services.AddSignalR();

            builder.Services.AddDbContext<RediDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
                .AddIdentity<UserBase, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Lockout.AllowedForNewUsers = false;
                    options.Lockout.AllowedForNewUsers = false;

                    options.User.AllowedUserNameCharacters += " ";

                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;

                    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                })
                .AddEntityFrameworkStores<RediDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IJwtGenerator, JwtService>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();

            builder.Services.AddTransient<DbSeeder>();

            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/delivery-chat") || path.StartsWithSegments("/delivery-track")))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddMvcCore(options => options.EnableEndpointRouting = false);

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/redi/swagger.json", "Redi API v1");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

                seeder.SeedAll();
            }

            app.MapHub<ChatHub>("/delivery-chat");
            app.MapHub<DeliveryHub>("/delivery-track");

            app.Run();
        }
    }
}