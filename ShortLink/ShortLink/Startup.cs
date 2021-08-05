using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShortLink.Models;

namespace ShortLink
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
            services.AddDbContext<ShortLinkDBContext>(options =>
                options.UseSqlServer("Server=.;Database=ShortLinkDB;Trusted_Connection=True;")
                //options.UseSqlServer("server=.;database=ShortLinkDB;User Id=sa;Password=S33@||;")
                );
            services.AddDbContext<LinkDBContext>(options =>
                options.UseSqlServer("Server=.;Database=EmployDB;Trusted_Connection=True;")
                //options.UseSqlServer("server=.;database=EmployDB;User Id=sa;Password=S33@||;")
            );
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

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
