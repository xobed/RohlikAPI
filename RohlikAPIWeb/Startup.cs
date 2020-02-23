using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using RohlikAPIWeb.Model;
using RohlikAPIWeb.Monitoring;
using Sentry;

namespace RohlikAPIWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);

            var appSettings = Configuration.Get<AppSettings>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = appSettings.GetRedisSettings();
            });
            services.AddSingleton<ISentryClient, SentryClient>();
            services.AddSingleton<DataDogClient>();
            services.AddSingleton<RedisStorage>();
            services.AddHostedService<RohlikProductsUpdateService>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 2;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromHours(1)
                    };
                context.Response.Headers[HeaderNames.Vary] =
                    new[] {"Accept-Encoding"};

                await next();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}