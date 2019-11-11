using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mobile.Core.Engines.Dependency;
using Mobile.Core.Engines.Services;
using Mobile.Core.ViewModels;
using MobileApp.Service;
using System;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class App : Application
    {
        public App(INavigationService navigation)
        {
            InitializeComponent();
            var vs = new ViewService();
            vs.Init(Locator.GetInstance<INavigationService>());
            navigation.Init<SplashViewModel>();
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
            services.AddSingleton<INavigationService, NavigationService>();
            Locator.Build(services);
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
