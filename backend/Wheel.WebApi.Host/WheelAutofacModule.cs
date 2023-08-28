using Autofac;
using System.Reflection;
using Wheel.DependencyInjection;
using Wheel.Domain;
using Wheel.EntityFrameworkCore;
using Module = Autofac.Module;

namespace Wheel
{
    public class WheelAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //把服务的注入规则写在这里
            builder.RegisterGeneric(typeof(EFBasicRepository<,>)).As(typeof(IBasicRepository<,>)).InstancePerDependency();

            var abs = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                        .Where(x => !x.Contains("Microsoft.") && !x.Contains("System."))
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x))).ToArray();

            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(IScopeDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(abs)
                .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
