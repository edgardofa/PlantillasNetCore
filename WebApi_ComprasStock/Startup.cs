using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApi_ComprasStock.Data;
using WebApi_ComprasStock.Entidades;
using WebApi_ComprasStock.Filtros;
using WebApi_ComprasStock.Servicios;
using WebApi_ComprasStock.Utilidades;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace WebApi_ComprasStock
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

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(provider =>
               new MapperConfiguration(config =>
               {
                   config.AddProfile(new AutoMapperProfiles());
               }).CreateMapper());

            //..............................................................................................
            ////Para Guardar archivos en Azure Storage
            //services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();
            //.........................................................................

            //.........................................................................
            //Para Guardar archivos en el servidor WEB
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            services.AddHttpContextAccessor();
            //..............................................................................................
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SistemasConnection"),
                sqlserver => sqlserver.UseNetTopologySuite())
            );
            //..............................................................................................
            var frontendURL = Configuration.GetValue<string>("frontend_url");
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });
            //..............................................................................................
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones =>
                {
                    opciones.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            //........................................................................................
            services.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("EsAdmin", policy => policy.RequireClaim("role", "admin"));
            });
            //........................................................................................
            services.AddControllers(opciones =>
                {
                    opciones.Filters.Add(typeof(FiltroDeExcepcion));
                })
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //........................................................................................
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi_ComprasStock", Version = "v1" });

                var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXML = Path.Combine(AppContext.BaseDirectory, archivoXML);
                c.IncludeXmlComments(rutaXML);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi_ComprasStock v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
