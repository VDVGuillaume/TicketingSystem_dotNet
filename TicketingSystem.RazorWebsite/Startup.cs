using Autofac;
using MediatR;
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
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Infrastructure;
using TicketingSystem.Infrastructure.QueryHandlers;

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
            services.AddMediatR(typeof(Startup));

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Lockout.DefaultLockoutTimeSpan = DateTime.Now.AddYears(10) - DateTime.Now;
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<TicketingSystemDbContext>();
            services.AddRazorPages();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Mediator itself
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
                   pattern: "{controller=Ticket}/{action=Index}/{id?}");
            });

            dataInitializer.InitializeData();
        }
    }
}
