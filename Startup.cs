using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RecipeApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Npgsql;
using System.Data;
using RecipeApp.Settings;
using RecipeApp.Services;
using System;

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
            IConfigurationSection mailSettings = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);

            services.AddScoped<IDbConnection>(e => new NpgsqlConnection(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();

            services.AddTransient<IMailService, MailKitService>();
            
            // Had to add these next two because I'm using AddIdeneityCore instead of AddIdeneity
            // See if this is the right thing to do. With AddIdeneity you are forced to have a RoleStore, which I didn't want
            // But now I see that using IdeneityCore means I have to register things on myown
            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ExternalScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.ExternalScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });
            services.AddSingleton<ISystemClock, SystemClock>();            
            
            services.AddIdentityCore<ApplicationUser>()
                .AddSignInManager()
                .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options => {
                options.SignIn.RequireConfirmedAccount = true;
            });

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
