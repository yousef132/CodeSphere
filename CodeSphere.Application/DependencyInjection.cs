using CodeSphere.Application.Behaviors;
using CodeSphere.Application.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeSphere.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            var assembly = typeof(DependencyInjection).Assembly;
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<ElasticSetting>(configuration.GetSection("ElasticSearch"));
            //services.Configure<JwtOptions>(configuration.GetSection("JWT"));

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();



            // redis
            services.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetConnectionString("Redis"));


            // mediator
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));

            // fluent validations
            services.AddValidatorsFromAssembly(assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //         services.AddTransient<IUrlHelper>(x =>
            //{
            //	var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            //	var factory = x.GetRequiredService<IUrlHelperFactory>();
            //	return factory.GetUrlHelper(actionContext);
            //});


            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            // auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
