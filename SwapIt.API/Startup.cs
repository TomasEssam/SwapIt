using AutoMapper;
using SwapIt.BL.Helpers;
using SwapIt.BL.IServices;
using SwapIt.BL.Services;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.Helpers;
using SwapIt.Data.IRepository;
using SwapIt.Data.Repository;

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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
            services.AddScoped<IUserBalanceRepository, UserBalanceRepository>();
            services.AddScoped<IUserBalanceService, UserBalanceService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceRequestService, ServiceRequestService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddDbContext<SwapItDbContext>();

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new GeneralProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();

            //services.AddSingleton(mapper);

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
