﻿using Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Wheel.Administrator.EntityFrameworkCore;
using Wheel.DependencyInjection;
using Wheel.Domain;
using Module = Autofac.Module;

namespace Wheel.Administrator
{
    public class AdministratorAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //把服务的注入规则写在这里
            builder.RegisterGeneric(typeof(EFBasicRepository<,>)).As(typeof(IBasicRepository<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(EFBasicRepository<>)).As(typeof(IBasicRepository<>)).InstancePerDependency();

            var abs = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                        .Where(x => !x.Contains("Microsoft.") && !x.Contains("System."))
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x))).ToArray();

            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency(); //瞬态
            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(IScopeDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope(); //范围
            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance(); //单例.


            // 获取所有控制器类型并使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(abs)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }
    }
}
