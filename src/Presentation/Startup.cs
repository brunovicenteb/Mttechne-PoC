using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mttechne.Infra.IoC;
using Mttechne.Infra.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Mttechne.Toolkit;
using System.Data;

namespace Mttechne.UI.Web;

public class Startup
{
    private const string DataBaseConnectionVarName = "DATABASE_CONNECTION";
    public IConfiguration Configuration { get; }


    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews(opt =>
            {
                opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddRazorRuntimeCompilation()
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        var stringConnection = EnvironmentReader.Read<string>(DataBaseConnectionVarName,
            varEmptyError: $"Unable to identify DATABASE_CONNECTION variable. Unable to start the application.");
        services.RegisterServices(stringConnection);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePagesWithRedirects("/Errors/PageNotFound");
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}