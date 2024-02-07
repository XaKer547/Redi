using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Redi.Api.Infrastructure;
using Redi.Api.Infrastructure.Interfaces;
using Redi.DataAccess.Data;
using Redi.DataAccess.Data.Entities;
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
                c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
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

            builder.Services.AddSignalRCore();

            builder.Services.AddDbContext<RediDbContext>(opts => opts.UseInMemoryDatabase("test"))
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Lockout.AllowedForNewUsers = false;
                    options.Lockout.AllowedForNewUsers = false;

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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddMvcCore(options => options.EnableEndpointRouting = false);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/redi/swagger.json", "Redi API v1");
                    c.DocExpansion(DocExpansion.None);
                });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var user = new User()
                {
                    Email = "FIFA228Nothack@gmail.com",
                    NormalizedEmail = "FIFA228Nothack@gmail.com".ToLower(),
                    UserName = "XaKer_547"
                };
                userManager.CreateAsync(user, "Qwe123456").Wait();
            }
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<ChatHub>("/chat");
            //});

            /*
{
"email": "fifa228nothack@gmail.com",
"password": "Qwe123456"
} 
 */
            app.Run();
        }
    }
}
