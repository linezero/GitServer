using GitServer.Services;
using GitServer.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GitServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using GitServer.Handlers;
using GitServer.ApplicationCore.Interfaces;
using NetCoreBBS.Infrastructure.Repositorys;
using GitServer.ApplicationCore.Models;
using Microsoft.Extensions.Hosting;

namespace GitServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionType = Configuration.GetConnectionString("ConnectionType");
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            switch (connectionType)
            {
                case "Sqlite":
                    services.AddDbContext<GitServerContext>(options => options.UseSqlite(connectionString));
                    break;
                case "MSSQL":
                    services.AddDbContext<GitServerContext>(options => options.UseSqlServer(connectionString));
                    break;
                case "MySQL":
                    services.AddDbContext<GitServerContext>(options => options.UseMySQL(connectionString));
                    break;
                default:
                    services.AddDbContext<GitServerContext>(options => options.UseSqlite(connectionString));
                    break;
            }
            // Add framework services.
            services.AddMvc(option=> {
                option.EnableEndpointRouting = false;
            });
			services.AddOptions();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options=> {
                options.AccessDeniedPath = "/User/Login";
                options.LoginPath = "/User/Login";
            }).AddBasic();

            // Add settings
            services.Configure<GitSettings>(Configuration.GetSection(nameof(GitSettings)));
			// Add git services
			services.AddTransient<GitRepositoryService>();
			services.AddTransient<GitFileService>();
            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IRepository<Repository>, Repository<Repository>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeGitServerDatabase(app.ApplicationServices);
			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error");
			}
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc(routes => RouteConfig.RegisterRoutes(routes));
		}
        private void InitializeGitServerDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<GitServerContext>();
                db.Database.EnsureCreated();
                if (db.Users.Count() == 0)
                {
                    //db.SaveChanges();
                }
            }
        }
    }
}
