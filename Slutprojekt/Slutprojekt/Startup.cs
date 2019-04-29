using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slutprojekt.Models;

namespace Slutprojekt
{
    public class Startup
    {

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Read the connection string (from appsettings.json during dev)
            var connString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<VeganIdentityContext>(o => o.UseSqlServer(connString));
            services.AddIdentity<VeganIdentityUser, IdentityRole>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<VeganIdentityContext>()
            .AddDefaultTokenProviders();

            // This is only needed if your login path should be anything else than "/Account/Login" 
            services.ConfigureApplicationCookie(o => o.LoginPath = "/LogIn");

            services.AddTransient<VeganService>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
