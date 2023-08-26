using LoyalHealthAPI.Interfaces;
using LoyalHealthAPI.Models;
using LoyalHealthAPI.Models.Interfaces;
using LoyalHealthAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace LoyalHealthAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _fileName;
        private readonly int _keySize = new Random().Next(3, 6);
        private readonly int _outPutSize = new Random().Next(200, 500);
        private string _dataFilePath;

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
            _dataFilePath = $"{_webHostEnvironment.ContentRootPath}/Files/{_fileName}";
            services.AddSingleton((IMarkovChainTextGenerator)new MarkovChainTextGenerator(new TrainingData { ReviewData = loadTrainingData(_keySize), KeySize = _keySize, OutputSize = _outPutSize }));
            services.AddControllers();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoyalHealthAPI", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoyalHealthAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Dictionary<string, List<string>> loadTrainingData(int keySize)
        {
            var reviews = new List<string>();
            using (var fileStream = File.OpenRead(_dataFilePath))
            {
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    using (var streamReader = new StreamReader(gzipStream))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                JObject reviewData = JObject.Parse(line);
                                if (reviewData.ContainsKey("reviewText"))
                                {
                                    reviews.Add(reviewData["reviewText"].ToString());
                                }
                            }
                        }
                    }
                }
            }

            var allReviewText = string.Join(" ", reviews).Split();

            Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < allReviewText.Length - keySize; i++)
            {
                var key = allReviewText.Skip(i).Take(keySize).Aggregate(createKeyFromPrefixWithSuffix);
                string value;
                if (i + keySize < allReviewText.Length)
                {
                    value = allReviewText[i + keySize];
                }
                else
                {
                    value = "";
                }

                if (dataDictionary.ContainsKey(key))
                {
                    dataDictionary[key].Add(value);
                }
                else
                {
                    dataDictionary.Add(key, new List<string>() { value });
                }
            }
            return dataDictionary;

        }

        private string createKeyFromPrefixWithSuffix(string prefix, string suffix)
        {
            return $"{prefix} {suffix}";
        }

    }
}
