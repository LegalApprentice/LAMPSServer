using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LAMPSServer;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

// hoping this will fix wwwroot directory on a Mac
// https://stackoverflow.com/questions/53653926/aspnet-core-macos-dev-cant-find-appsettings-and-wwwroot
// Also, in macOS Directory.GetCurrentDirectory() will return <ProjectRoot>/bin/debug/net462 and windows will return <ProjectRoot>.

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

