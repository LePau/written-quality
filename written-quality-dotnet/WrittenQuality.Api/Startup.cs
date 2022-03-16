using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WrittenQuality.Models;
using WrittenQuality.Services;

namespace WrittenQuality.Api
{
    public class Startup
    {
        private string _metadataServiceKey = null;
        private string _metadataServiceDomain = null;
        private string _googleApplicationCredentials = null;
        private string _googleApplicationCredentialsPath = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _metadataServiceKey = System.Environment.GetEnvironmentVariable("METADATA_SERVICE_KEY");
            _metadataServiceDomain = System.Environment.GetEnvironmentVariable("METADATA_SERVICE_DOMAIN");
            _googleApplicationCredentials = System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_JSON");
            _googleApplicationCredentialsPath = System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

            // limitation in gcloud command line, where json strings can't have commas
            _googleApplicationCredentials = _googleApplicationCredentials?.Replace(";;;", ",");

            var hasGoogleCredentials = _googleApplicationCredentials != null;
            var hasGoogleCredentialsPath = _googleApplicationCredentialsPath != null;
            Console.WriteLine($"Has google credentials Path:{hasGoogleCredentialsPath}, Creds: {hasGoogleCredentials}");
            if (hasGoogleCredentials && hasGoogleCredentialsPath )
            {
                Console.WriteLine($"Writing google credentials to {_googleApplicationCredentialsPath}");
                System.IO.File.WriteAllText(_googleApplicationCredentialsPath, _googleApplicationCredentials);
                Console.WriteLine($"Wrote {_googleApplicationCredentials.Truncate(100)} to {_googleApplicationCredentialsPath}");
                var sample = System.IO.File.ReadAllText(_googleApplicationCredentialsPath);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IScrapeService>(serviceProvider => new ScrapeService(serviceProvider.GetService<ILogger<ScrapeService>>(), _metadataServiceDomain, _metadataServiceKey));
            services.AddSingleton<INlpService>(serviceProvider => new NlpService(serviceProvider.GetService<ILogger<NlpService>>()));

            services.AddScoped<Logic.IWrittenDocument, Logic.WrittenDocument>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WrittenQuality.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WrittenQuality.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors(builder => builder.AllowAnyMethod().WithHeaders("content-type", "Authorization", "Request-Id", "From").AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
