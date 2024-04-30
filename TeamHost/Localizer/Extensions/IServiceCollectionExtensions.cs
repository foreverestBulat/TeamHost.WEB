using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace TeamHost.Localizer.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddLocalizer(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Localizer/Resources");
            services.Configure();
        }

        public static void Configure(this IServiceCollection services)
        {
            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("th"),
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
        }
    }

    //app.UseRequestLocalization(new RequestLocalizationOptions
    //{
    //    DefaultRequestCulture = new RequestCulture("en"),
    //    SupportedCultures = supportedCultures,
    //    SupportedUICultures = supportedCultures,
    //    RequestCultureProviders = new List<IRequestCultureProvider>
    //        {
    //            new QueryStringRequestCultureProvider(),
    //            new CookieRequestCultureProvider()
    //        }

    //});
}
