using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CodeSharingPlatform; // Added using directive for the CodeSharingPlatform namespace

namespace CodeSharingPlatform
{
    /// <summary>
    /// Program class to configure and run the web host.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the host builder with default configuration and startup class.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>An IHostBuilder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                { 
                    webBuilder.UseStartup<Startup>()
                              .UseUrls("http://0.0.0.0:5000"); // Listen on all network interfaces on port 5000
                }); // Ensure Startup is recognized

    }
}
