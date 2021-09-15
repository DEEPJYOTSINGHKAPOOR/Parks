using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using LearningDotNetCore.ParkyMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
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
            

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            

            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAutoMapper(typeof(ParkyMappings));


            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });


            services.AddSwaggerGen(options=> {
                //we want to add swagger document -open Api spec
                options.SwaggerDoc("ParkyOpenApiSpec",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Parky API",
                        Version = "1"
                    });
            });


            ///
            var appSettingsSection = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);


            services.AddAuthentication(
                x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


            //services.AddTransient<IOperationTransient, Operation>();

            //services.AddScoped<IOperationScoped, Operation>();

            //services.AddSingleton<IOperationSingleton, Operation>();

            //services.AddSingleton<IOperationSingletonInstance>(a => new Operation(Guid.Empty));


            //services.AddTransient<DependencyService1, DependencyService1>();
            //services.AddTransient<DependencyService2, DependencyService2>();


            //services.AddRazorPages();

            services.AddControllers();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();


            app.UseSwagger();

            //app.UseSwaggerUI(options => {
            //    foreach (var desc in provider.ApiVersionDescriptions)
            //        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
            //            desc.GroupName.ToUpperInvariant());
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenApiSpec/swagger.json","Parky API");
            //    options.RoutePrefix = "";
            //});

            app.UseStaticFiles();

            app.UseRouting();


            //cors - 
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );

            /// write authentication before authorization in the pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
//  : https://localhost:44342/swagger/ParkyOpenApiSpec/swagger.json