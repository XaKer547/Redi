using Microsoft.AspNetCore.Authentication.Cookies;
using Redi.Application.Services;
using Redi.Deliverer.Handlers;
using Redi.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<ApiAuthenticationHttpClientHandler>();

builder.Services.AddHttpClient<IRediApiProvider, RediApiProvider>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["Api"]))
    .AddHttpMessageHandler<ApiAuthenticationHttpClientHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authorization}/{action=Index}");

app.Run();
