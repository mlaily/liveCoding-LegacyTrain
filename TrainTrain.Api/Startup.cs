using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainTrain.Infra;

namespace TrainTrain.Api
{
    public class Startup
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // 1. Instantiate adapters to go out
            var trainDataService = new TrainDataService(UriTrainDataService);
            var bookingReferenceService = new BookingReferenceService(UriBookingReferenceService);

            // 2. instantiate hexagon
            var hexagon = new Hexagon(trainDataService, bookingReferenceService);

            // 3. Instantiate the "I need to enter/ask" adapter
            var reserveSeatsAdapter = new ReserveSeatsRestAdapter(hexagon);

            // All your application keeps is a reference to the "I need to enter/ask" adapters
            services.AddSingleton<ReserveSeatsRestAdapter>(reserveSeatsAdapter);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
        }
    }
}
