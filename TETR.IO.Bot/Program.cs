using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TETR.IO.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            if (!System.IO.File.Exists("TetrSetting.json"))
            {
                System.IO.File.WriteAllText("TetrSetting.json", JsonSerializer.Serialize(new BotSetting(), options));
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:47326");
                });
    }
}
