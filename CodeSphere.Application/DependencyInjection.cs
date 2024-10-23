using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeSphere.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // mediator
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));

            // fluent validations
            services.AddValidatorsFromAssembly(assembly);

            // auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
