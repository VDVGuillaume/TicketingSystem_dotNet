using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketingSystem.Infrastructure;
using Autofac;
using TicketingSystem.Domain.Application;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystemApi
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
            services.AddControllers();

            services.AddDbContext<TicketingSystemDbContext>(options => options.UseSqlServer(Configuration.GetValue<string>("SqlConnectionString")));
            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        public void ConfigureContainer(ContainerBuilder builder) 
        {
            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new InfrastructureModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Ensure database is created
            using (var scope = app.ApplicationServices.CreateScope()) 
            {
                using (var context = scope.ServiceProvider.GetService<TicketingSystemDbContext>())
                { 
                    context.Database.EnsureCreated(); 
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
        }
    }
}
