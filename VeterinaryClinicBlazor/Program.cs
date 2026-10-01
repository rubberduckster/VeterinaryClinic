using VeterinaryClinic.Repositories;
using VeterinaryClinic.Services;
using VeterinaryClinicBlazor.Components;
using VeterinaryClinicBlazor.Services;

namespace VeterinaryClinicBlazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            string connectionString = builder.Configuration
                .GetConnectionString("VeterinaryClinic")
                ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddScoped<DatabaseRepository>(provider =>
                new DatabaseRepository(connectionString));

            builder.Services.AddScoped<DatabaseService>();
            builder.Services.AddScoped<LanguageState>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
