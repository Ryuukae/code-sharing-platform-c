using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace CodeSharingPlatform
{
    /// <summary>
    /// Configures services and the app's request pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures services for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Serve static files from Views folder root with no request path prefix
            var viewsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Views");
            app.UseStaticFiles(new Microsoft.AspNetCore.Builder.StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(viewsFolder),
                RequestPath = ""
            });

            /// <summary>
            /// Temporarily disable HTTPS redirection to avoid redirect issues with 0.0.0.0 binding
            /// </summary>
            // app.UseHttpsRedirection();

            app.UseRouting();

            /// <summary>
            /// Adds authorization middleware to the request pipeline.
            /// </summary>
            app.UseAuthorization();

            /// <summary>
            /// Configures endpoint routing for the application.
            /// </summary>
            app.UseEndpoints(endpoints =>
            {
                /// <summary>
                /// Redirects root "/" to /code/latest.
                /// </summary>
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/code/latest");
                    return System.Threading.Tasks.Task.CompletedTask;
                });

                /// <summary>
                /// Redirects /latestsnippets.htmlcs to /code/latest.
                /// </summary>
                endpoints.MapGet("/latestsnippets.htmlcs", context =>
                {
                    context.Response.Redirect("/code/latest");
                    return System.Threading.Tasks.Task.CompletedTask;
                });

                /// <summary>
                /// Redirects /latestsnippets to /code/latest.
                /// </summary>
                endpoints.MapGet("/latestsnippets", context =>
                {
                    context.Response.Redirect("/code/latest");
                    return System.Threading.Tasks.Task.CompletedTask;
                });

                /// <summary>
                /// Maps route for viewing a code snippet by ID.
                /// </summary>
                endpoints.MapControllerRoute(
                    name: "code_view",
                    pattern: "code/view/{id}",
                    defaults: new { controller = "WebCode", action = "ViewSnippet" });

                /// <summary>
                /// Maps the default route to WebCode controller's Latest action.
                /// </summary>
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=WebCode}/{action=Latest}/{id?}");
            });
        }
    }
}
