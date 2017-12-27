using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using LibOwin;
using Microsoft.IdentityModel.Tokens;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.TinyIoc;
using Serilog;
using Serilog.Events;
using Toffees.Glucose.Data;
using Toffees.Glucose.Models.DTOs;
using ILogger = Serilog.ILogger;

namespace Toffees.Glucose
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly ILogger _log;

        public Bootstrapper(ILogger log)
        {
            _log = log;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Register(_log);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var correlationToken = context.GetOwinEnvironment()["correlationToken"] as string;
            //context.CurrentUser = context.GetOwinEnvironment()["pos-end-user"] as ClaimsPrincipal;
            //container.Register<IHttpClientFactory>(new HttpClientFactory(correlationToken));
        }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services
            //    .AddMvcCore()
            //    .AddJsonFormatters()
            //    .AddAuthorization();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<GlucoseDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("GlucoseDbContext")));
            services.AddTransient(provider => UrlEncoder.Default);
            services.AddCors();
            services.AddDistributedMemoryCache();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://localhost:5000";
                    options.ApiName = "biometric_api";
                    options.ApiSecret = "glucoseSecret";
                    //options.InboundJwtClaimTypeMap = new Dictionary<string, string>();
                    options.EnableCaching = false;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync("An error occurred processing your authentication.");
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug(LogLevel.Trace);
            var log = ConfigureLogger();

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = "http://localhost:5000/resources",
                IssuerSigningKey = new X509SecurityKey(new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "IdentityServer4Auth.pfx"))),
                ValidIssuer = "http://localhost:5000",
                RequireExpirationTime = true
            };

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            app.UseAuthentication();
            //app.UseMvc();
            app.UseOwin(buildFunc =>
            {
                buildFunc(next => env =>
                {
                    var ctx = new OwinContext(env);
                    //if (!ctx.Request.Headers.ContainsKey("pos-end-user")) return next(env);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var authToken = ctx.Request.Headers["Authorization"].Substring(7);
                    
                    var userPrincipal =
                        tokenHandler.ValidateToken(authToken,
                            validationParameters,
                            out _);
                    ctx.Set("pos-end-user", userPrincipal);
                    return next(env);
                });
                buildFunc(next => env =>
                {
                    var ctx = new OwinContext(env);
                    var principal = ctx.Request.User;
                    if (principal?.HasClaim("scope", "biometric_api.full_access") ?? false)
                    {
                        return next(env);
                    }
                    ctx.Response.StatusCode = 403;
                    return Task.FromResult(0);
                });
                buildFunc(next => env =>
                {
                    var ctx = new OwinContext(env);
                    var idToken = ctx.Request.User?.FindFirst("id_token");
                    if (idToken != null)
                    {
                        ctx.Set("pos-end-user-token", idToken);
                    }
                    return next(env);
                });

                buildFunc(next => GlobalErrorLogging.Middleware(next, log));
                buildFunc(CorrelationToken.Middleware);
                buildFunc(next => RequestLogging.Middleware(next, log));
                buildFunc(next => PerformanceLogging.Middleware(next, log));
                //buildFunc(next => new MonitoringMiddleware(next, HealthCheckTask).Invoke);
                buildFunc.UseNancy(opt => opt.Bootstrapper = new Bootstrapper(log));
            });

            Mapper.Initialize(mapperConfig =>
            {
                mapperConfig.CreateMap<Data.Entities.Glucose, GlucoseDto>().ReverseMap();
            });
        }

        private static ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.ColoredConsole(
                 LogEventLevel.Verbose,
                 "{NewLine}{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
              .CreateLogger();
        }

        //private readonly int _threshold = -1;
        //private IGlucosesRepository _activitiesRepository;

        //public async Task<bool> HealthCheckTask()
        //{
        //    var options = new DbContextOptions<GlucosesContext>();
        //    var ctx = new GlucosesContext(options);
        //    _activitiesRepository = new EfGlucosesRepository(ctx);
        //    var amountOfActivities = await _activitiesRepository.HealthCheckTask().ConfigureAwait(false);
        //    return amountOfActivities.Count > _threshold;
        //}
    }
}
