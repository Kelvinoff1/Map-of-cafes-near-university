using MapOfCafesNearUniversity.ServiceContracts;
using MapOfCafesNearUniversity.Services;
using MapOfCafesNearUniversity.Settings;

namespace MapOfCafesNearUniversity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMemoryCache();
            builder.Services.Configure<OverpassApiSettings>(builder.Configuration.GetSection("OverpassApi"));
            builder.Services.AddHttpClient<OverpassApiClient>();
            builder.Services.AddScoped<ILeafletService, LeafletService>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
