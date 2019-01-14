using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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


        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
