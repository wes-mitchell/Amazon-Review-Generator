using AmazonReviewGenerator.API.Models;
using AmazonReviewGenerator.API.Models.Interfaces;
using AmazonReviewGenerator.API.Services;
using AmazonReviewGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace AmazonReviewGenerator.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _fileName;
        private readonly int _keySize = new Random().Next(3, 6);
        private readonly int _outPutSize = new Random().Next(200, 500);

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _fileName = configuration["ReviewDataFileName"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var _dataFilePath = $"{_webHostEnvironment.ContentRootPath}/Files/{_fileName}";
            services.AddSingleton((IMarkovChainTextGenerator)new MarkovChainTextGenerator(new TrainingData 
            { 
                ReviewData = MarkovChainTextGenerator.LoadTrainingData(_keySize, _dataFilePath), KeySize = _keySize, OutputSize = _outPutSize 
            }));
            services.AddControllers();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmazonReviewGeneratorAPI", Version = "v1" });
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmazonReviewGeneratorAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
