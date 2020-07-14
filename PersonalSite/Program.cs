using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LettuceEncrypt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PersonalSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // This example shows how to configure Kestrel's client certificate requirements along with
                    // enabling Lettuce Encrypt's certificate automation.
                    if (Environment.GetEnvironmentVariable("REQUIRE_CLIENT_CERT") == "true")
                    {
                        webBuilder.UseKestrel(serverOptions =>
                        {
                            var appServices = serverOptions.ApplicationServices;
                            serverOptions.ConfigureHttpsDefaults(h =>
                            {
                                h.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                                h.UseLettuceEncrypt(appServices);
                            });
                        });
                    }

                    // This example shows how to configure Kestrel's address/port binding along with
                    // enabling Lettuce Encrypt's certificate automation.
                    if (Environment.GetEnvironmentVariable("CONFIG_KESTREL_VIA_CODE") == "true")
                    {
                        webBuilder.PreferHostingUrls(false);
                        webBuilder.UseKestrel(serverOptions =>
                        {
                            var appServices = serverOptions.ApplicationServices;
                            serverOptions.Listen(IPAddress.Any, 443, 
                                o => o.UseHttps(
                                    h => h.UseLettuceEncrypt(appServices)));
                            serverOptions.Listen(IPAddress.Any, 1337, 
                                o => o.UseHttps(
                                    h => h.UseLettuceEncrypt(appServices)));
                            serverOptions.Listen(IPAddress.Any, 80);
                        });
                    }
                });
    }
}
