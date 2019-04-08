using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace asp_webapi {
  public class Program {
    public static void Main (string[] args) {
      var logRepository = log4net.LogManager.GetRepository (Assembly.GetEntryAssembly ());
      log4net.Config.XmlConfigurator.Configure (logRepository, new FileInfo ("log4net.config"));
      CreateWebHostBuilder (args).Build ().Run ();
    }

    public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
      WebHost.CreateDefaultBuilder (args)
      .UseStartup<Startup> ();
  }
}