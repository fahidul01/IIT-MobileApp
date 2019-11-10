﻿using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using Microsoft.Extensions.DependencyInjection;
using Mobile.Core.Engines.APIHandlers;
using Mobile.Core.ViewModels;
using Mobile.Core.Worker;
using System;
using System.Linq;
using System.Reflection;

namespace Mobile.Core.Engines.Dependency
{
    public static class Locator
    {
        private static IServiceProvider provider;
        public static T GetInstance<T>()
        {
            return (T)provider.GetService(typeof(T));
        }

        public static void Init(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
        }



        public static void Build(IServiceCollection services)
        {
            RegisterAllTypes<BaseViewModel>(services, typeof(BaseViewModel).Assembly);
            services.AddSingleton<ILessonHandler, LessonEngine>();
            services.AddSingleton(new HttpWorker(AppConstants.BaseUrl));
        }

        public static void RegisterAllTypes<T>(IServiceCollection services, Assembly assembly)
        {

            var types = assembly.GetTypes()
                                .Where(myType => myType.IsClass &&
                                      !myType.IsAbstract &&
                                      myType.IsSubclassOf(typeof(T)));

            foreach (var type in types)
            {
                services.AddTransient(type);
            }
        }
    }
}
