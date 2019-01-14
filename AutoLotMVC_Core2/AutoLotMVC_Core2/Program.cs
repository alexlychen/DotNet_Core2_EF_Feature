using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoLotDAL_Core2.DataInitialization;
using AutoLotDAL_Core2.EF;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoLotMVC_Core2
{
    public class Program
    {
        private static bool RecreateDatabase = false;

        public static void Main(string[] args)
        {
            
            if (!RecreateDatabase)
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            else
            {
                /* In case to recreate the database */
                var webHost = CreateWebHostBuilder(args).Build();
                using (var scope = webHost.Services.CreateScope()) //use Dependency Injection
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<AutoLotContext>();
                    MyDataInitializer.RecreateDatabase(context);
                    MyDataInitializer.InitializeData(context);
                }
                webHost.Run();
            }


        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
