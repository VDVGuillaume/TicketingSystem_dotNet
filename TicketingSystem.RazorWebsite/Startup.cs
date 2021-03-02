using Autofac;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure;
using TicketingSystem.Infrastructure.Services;
using TicketingSystem.Infrastructure.Services.Interfaces;
using TicketingSystem.RazorWebsite.Mapping;

namespace TicketingSystem.RazorWebsite
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
            services.AddDbContext<TicketingSystemDbContext>(options => options.UseSqlServer(Configuration.GetValue<string>("SqlConnectionString")));
            services.AddScoped<TicketingSystemDataInitializer>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddMediatR(typeof(Startup));

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Lockout.DefaultLockoutTimeSpan = DateTime.Now.AddYears(10) - DateTime.Now;
                options.Lockout.MaxFailedAccessAttempts = 5;
            }) 
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TicketingSystemDbContext>();
            services.AddRazorPages();
            services.AddControllers();
            services.AddSignalR();

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opt =>
            {
                //configure your other properties
                opt.LoginPath = "/Account/Login";
            });

            //configure automapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            //// finally register our custom code (individually, or via assembly scanning)
            //// - requests & handlers as transient, i.e. InstancePerDependency()
            //// - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            //// - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(DomainModule).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            builder.RegisterAssemblyTypes(typeof(InfrastructureModule).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan

            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new InfrastructureModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TicketingSystemDataInitializer dataInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Dashboard}/{action=Index}");

                endpoints.MapRazorPages();
            });

            dataInitializer.InitializeData().Wait();
        }
    }
}
