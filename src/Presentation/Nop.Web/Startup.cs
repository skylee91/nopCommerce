﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nop.Core.Infrastructure;
using Nop.Core.Configuration;
using Nop.Web.Extensions;

namespace Nop.Web
{
    public class Startup
    {
        #region Properties

        /// <summary>
        /// Get configuration root
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        #endregion

        #region Ctor

        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        #endregion

        /// <summary>
        /// Add services to the container
        /// </summary>
        /// <param name="services">The contract for a collection of service descriptors</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //add options feature
            services.AddOptions();

            //add NopConfig configuration parameters
            var nopConfig = services.ConfigureStartupConfig<NopConfig>(Configuration.GetSection("Nop"));

            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(Configuration.GetSection("Hosting"));

            //initialize engine context
            EngineContext.Initialize(nopConfig, false);
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder that provides the mechanisms to configure an application's request pipeline</param>
        /// <param name="environment">Provides information about the web hosting environment an application is running in</param>
        /// <param name="loggerFactory">Object used to configure the logging system</param>
        public void Configure(IApplicationBuilder application, IHostingEnvironment environment, ILoggerFactory loggerFactory)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }

            application.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}