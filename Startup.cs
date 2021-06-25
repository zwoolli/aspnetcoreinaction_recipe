using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RecipeApp.Data;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Data;

namespace RecipeApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IDbConnection>(e => new NpgsqlConnection(connectionString));
            services.AddTransient<IUserRepository, UserRepository>();


            // services.AddScoped<IUserStore<ApplicationUser>>(x => new UserStore(connectionString));
            services.AddIdentityCore<ApplicationUser>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
