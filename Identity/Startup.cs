using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toffees.Identity.Data;
using Toffees.Identity.Models;

namespace Toffees.Identity
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //if (env.IsDevelopment())
            //{
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
            //}

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityServerContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApis())
            .AddInMemoryClients(Config.GetClients())
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(Configuration.GetConnectionString("IdentityServerContext"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(Configuration.GetConnectionString("IdentityServerContext"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //InitializeDatabase(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        //private void InitializeDatabase(IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        //        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        //        context.Database.Migrate();
        //        if (!context.Clients.Any())
        //        {
        //            foreach (var client in Config.GetClients())
        //            {
        //                context.Clients.Add(client.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }

        //        if (!context.IdentityResources.Any())
        //        {
        //            foreach (var resource in Config.GetIdentityResources())
        //            {
        //                context.IdentityResources.Add(resource.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }

        //        if (context.ApiResources.Any()) return;
        //        {
        //            foreach (var resource in Config.GetApis())
        //            {
        //                context.ApiResources.Add(resource.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }
        //    }
        //}
    }
}
