using DotNetCore.CAP.Internal;
using DotNetCore.CAP;
using Microsoft.Extensions.Options;
using System.Reflection;
using Autofac.Core;
using Wheel.DependencyInjection;

namespace Wheel.EventBus.Distributed.Cap
{
    public class WheelConsumerServiceSelector : ConsumerServiceSelector
    {
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Creates a new <see cref="T:DotNetCore.CAP.Internal.ConsumerServiceSelector" />.
        /// </summary>
        public WheelConsumerServiceSelector(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected override IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromInterfaceTypes(IServiceProvider provider)
        {
            var executorDescriptorList = base.FindConsumersFromInterfaceTypes(provider).ToList();

            using var scope = provider.CreateScope();
            var scopeProvider = scope.ServiceProvider;
            //handlers
            var handlers = scopeProvider.GetServices<IEventHandler>()
                     .Select(o => o.GetType()).ToList();

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                    {
                        continue;
                    }
                    var genericArgs = @interface.GetGenericArguments();

                    if (genericArgs.Length != 1)
                    {
                        continue;
                    }
                    if(!(@interface.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>)))
                    {
                        continue;
                    }

                    var descriptors = GetHandlerDescription(genericArgs[0], handler);

                    foreach (var descriptor in descriptors)
                    {
                        var count = executorDescriptorList.Count(x =>
                            x.Attribute.Name == descriptor.Attribute.Name);

                        descriptor.Attribute.Group = descriptor.Attribute.Group.Insert(
                            descriptor.Attribute.Group.LastIndexOf(".", StringComparison.Ordinal), $".{count}");

                        executorDescriptorList.Add(descriptor);
                    }
                }
            }
            return executorDescriptorList;
        }

        protected virtual IEnumerable<ConsumerExecutorDescriptor> GetHandlerDescription(Type eventType, Type typeInfo)
        {
            var serviceTypeInfo = typeof(IDistributedEventHandler<>)
                .MakeGenericType(eventType);
            var method = typeInfo
                .GetMethod(
                    nameof(IDistributedEventHandler<object>.Handle)
                );
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            var topicAttr = method.GetCustomAttributes<TopicAttribute>(true);
            var topicAttributes = topicAttr.ToList();

            if (topicAttributes.Count == 0)
            {
                topicAttributes.Add(new CapSubscribeAttribute(eventName));
            }

            foreach (var attr in topicAttributes)
            {
                SetSubscribeAttribute(attr);

                var parameters = method.GetParameters()
                    .Select(parameter => new ParameterDescriptor
                    {
                        Name = parameter.Name,
                        ParameterType = parameter.ParameterType,
                        IsFromCap = parameter.GetCustomAttributes(typeof(FromCapAttribute)).Any()
                                    || typeof(CancellationToken).IsAssignableFrom(parameter.ParameterType)
                    }).ToList();

                yield return InitDescriptor(attr, method, typeInfo.GetTypeInfo(), serviceTypeInfo.GetTypeInfo(), parameters);
            }
        }

        private static ConsumerExecutorDescriptor InitDescriptor(
            TopicAttribute attr,
            MethodInfo methodInfo,
            TypeInfo implType,
            TypeInfo serviceTypeInfo,
            IList<ParameterDescriptor> parameters)
        {
            var descriptor = new ConsumerExecutorDescriptor
            {
                Attribute = attr,
                MethodInfo = methodInfo,
                ImplTypeInfo = implType,
                ServiceTypeInfo = serviceTypeInfo,
                Parameters = parameters
            };

            return descriptor;
        }
    }
}
