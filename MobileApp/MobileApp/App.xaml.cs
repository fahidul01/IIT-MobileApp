using CoreEngine.Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mobile.Core.Engines.Dependency;
using Mobile.Core.Engines.Services;
using Mobile.Core.Models.Core;
using Mobile.Core.ViewModels;
using MobileApp.Controls;
using MobileApp.Service;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App(IPlatformService platformService)
        {
            InitializeComponent();
            var nav = new NavigationService(platformService);
            LogEngine.ErrorOccured += (s, e) => nav.ShowMessage("Error", e);
            RegisterPages(nav);
            AppService.Init(nav, nav);
            nav.Init<HomeViewModel>();
        }

        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var host = new HostBuilder()
                            //ConfigureHostConfiguration
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o => o.DisableColors = true))
                            .UseContentRoot(Environment.CurrentDirectory)
                            .Build();

            Locator.Init(host.Services);
            return Locator.GetInstance<App>();
        }

        private static void ConfigureServices(HostBuilderContext c, IServiceCollection services)
        {
            services.AddSingleton<App>();
            services.AddSingleton<IPreferenceEngine, PreferenceEngine>();
            Locator.Build(services);
        }

        private static void RegisterPages(INavigationService _nav)
        {
            var assembly = typeof(App).Assembly;
            //var types = assembly.GetTypes()
            //                   .Where(myType => myType.IsClass &&
            //                         !myType.IsAbstract &&
            //                         myType.IsSubclassOf(typeof(CustomPage<SplashScreenModel>)));

            var types = from x in Assembly.GetAssembly(typeof(App)).GetTypes()
                        let y = x.BaseType
                        where !x.IsAbstract && !x.IsInterface &&
                        y != null && y.IsGenericType &&
                        y.GetGenericTypeDefinition() == typeof(CustomPage<>)
                        select x;

            var tabTypes = from x in Assembly.GetAssembly(typeof(App)).GetTypes()
                           let y = x.BaseType
                           where !x.IsAbstract && !x.IsInterface &&
                           y != null && y.IsGenericType &&
                           y.GetGenericTypeDefinition() == typeof(CustomTabPage<>)
                           select x;

            foreach (var type in types)
            {
                var page = type.BaseType.GetGenericArguments().FirstOrDefault();
                _nav.Configure(page, type);
            }

            foreach (var type in tabTypes)
            {
                var page = type.BaseType.GetGenericArguments().FirstOrDefault();
                _nav.Configure(page, type);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
