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
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalRCore();

            builder.Services.AddDbContext<RediDbContext>(opt =>
            {
                //opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                opt.UseInMemoryDatabase("redi.db");
            }).AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
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
                    })
                    .AddEntityFrameworkStores<RediDbContext>()
                    .AddDefaultTokenProviders();

                builder.Services.AddScoped<IJwtGenerator, JwtService>();
                builder.Services.AddScoped<IMailService, MailService>();

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

                builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseDefaultFiles();
                app.UseStaticFiles();

                //app.UseHttpsRedirection();
                app.UseMvc();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();
                //app.UseEndpoints(endpoints =>
                //{
                //    endpoints.MapHub<ChatHub>("/chat");
                //});

                app.Run();
            }
    }
    }