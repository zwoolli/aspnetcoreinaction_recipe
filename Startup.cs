using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using RecipeApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
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

            // services.AddScoped<IDbConnection>(e => new NpgsqlConnection(connectionString));
            // Had to switch to passing in the connection string rather than the connection because I could not
            // Open a connection and dispose of it (in the using block) more than once otherwise. 
            services.AddScoped<IUserRepository>(x => new UserRepository(connectionString));
            services.AddScoped<IRecipeRepository>(x => new RecipeRepository(connectionString));
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();

            services.AddTransient<IMailService, MailKitService>();
            
            #region AddidentityCore
            // copied from the AddIdentity source code
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme);

            services.AddIdentityCore<ApplicationUser>(options => 
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddSignInManager<CustomSignInManager>()
            .AddDefaultTokenProviders();

            #endregion

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
