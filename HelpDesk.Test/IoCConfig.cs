using HelpDesk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using HelpDesk.Data.Repository.Interfaces;
using AutoMapper;
using HelpDesk.Api;
using HelpDesk.Helper.Interfaces;
using HelpDesk.Helper;
using HelpDesk.Bll.Components.Interfaces;
using HelpDesk.Bll.Components;
using HelpDesk.Bll.Interfaces;
using HelpDesk.Bll;
using System.Security.Claims;
using System.Security.Principal;

namespace HelpDesk.Test
{
    /// <summary>
    /// The IoCConfig class provide installing all components needed to use.
    /// </summary>
    public class IoCConfig
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCConfig" /> class.
        /// </summary>
        public IoCConfig()
        {
            // Load configuration file.
            var config = new ConfigurationBuilder()
                            .AddJsonFile(this.GetAppSettingDirectory())
                            .Build();

            var services = new ServiceCollection();
            //Add services to the container.
            services.AddEntityFrameworkNpgsql()
            .AddDbContext<HelpDeskContext>(options =>
             options.UseNpgsql(config["ConnectionStrings:DefaultConnection"]));

            RedisCacheHandler.ConnectionString = config["ConnectionStrings:RedisCacheConnection"];

            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<IUnitOfWork, HelpDeskUnitOfWork>();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<ILoginBll, LoginBll>();
            services.AddScoped<IRegisterBll, RegisterBll>();
            services.AddScoped<IPasswordBll, PasswordBll>();
            services.AddScoped<ICompanyBll, CompanyBll>();
            services.AddScoped<ITicketBll, TicketBll>();
            services.AddScoped<ITicketCommentBll, TicketCommentBll>();
            services.AddScoped<ITicketTransectionBll, TicketTransectionBll>();
            services.AddScoped<IPriorityBll, PriorityBll>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfigSetting, ConfigSetting>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddTransient<IManageToken, ManageToken>(c => new ManageToken(this.InitialHttpContext()));

            ServiceProvider = services.BuildServiceProvider();

        }

        /// <summary>
        /// Get appsetting json file in unit test directory.
        /// </summary>
        /// <returns>appsetting directory path.</returns>
        private string GetAppSettingDirectory()
        {
            return string.Concat(System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("bin")), "appsettings.json");
        }

        /// <summary>
        /// The Serivce Provider, this provides access to the IServiceCollection.
        /// </summary>
        public ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Initial Mockup HttpContext inject to test.
        /// </summary>
        /// <returns></returns>
        private HttpContextAccessor InitialHttpContext()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var httpContext = new DefaultHttpContext();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "nuttaphon@leaderplanet.co.th"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
            identity.AddClaim(new Claim(ConstantValue.ClamisComCode, "1000"));
            identity.AddClaim(new Claim(ConstantValue.ClamisFullName, string.Format(ConstantValue.EmpTemplate, "admin", "admin")));
            var user = new GenericPrincipal(new ClaimsIdentity(identity), new string[] { "ADMIN" });
            httpContext.User = user;

            httpContextAccessor.HttpContext = httpContext;
            return httpContextAccessor;
        }



    }
}
