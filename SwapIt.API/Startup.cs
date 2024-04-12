using SwapIt.Data.Helpers;

namespace SwapIt.API
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
            //services.AddCors(Configuration);
            services.AddDbContext(Configuration);
            services.AddIdentity();
            //services.AddAuthenticationConfiguration(Configuration);
            services.AddMapper();
            services.AddApplicationServices();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("CORSDev");
            }
            else
            {
                app.UseCors("CORS");
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cura V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            AppSecurityContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        }
    }
}
