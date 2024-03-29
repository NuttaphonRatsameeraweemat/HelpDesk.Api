﻿using AutoMapper;
using HelpDesk.Api.Extensions;
using HelpDesk.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            NLog.LogManager.LoadConfiguration(string.Concat(System.IO.Directory.GetCurrentDirectory(), "/NLog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Configure Extension and Bll class.
            services.ConfigureRepository(Configuration);
            services.ConfigureRedisCache(Configuration);
            services.ConfigureBll();
            services.ConfigureHttpContextAccessor();
            services.ConfigureLoggerService();
            services.ConfigureCors();
            //services.ConfigureJwtAuthen(Configuration);
            services.ConfigureCookieAuthen(Configuration);
            services.ConfigureEmailService();
            services.ConfigureComponent();
            services.AddAutoMapper();
            services.AddMvc(opt =>
            {
                opt.UseApiGlobalConfigRoutePrefix(new RouteAttribute("api"));
                opt.Filters.Add(typeof(ValidateModelStateAttribute));

            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.ffffzzz";
            });
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwaager();
            app.UseAuthentication();
            app.ConfigureMiddleware();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
