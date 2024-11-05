using Microsoft.OpenApi.Models;

namespace CodeSphere.WebApi.Extentions
{
    public static class SwaggerServicesExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        public static WebApplication UseSwaggerService(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }


        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Talabat API",
                    Version = "v1",
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "Some Description",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };
                options.AddSecurityDefinition("bearer", securityScheme);

                var securityRequirments = new OpenApiSecurityRequirement
                {
                    {securityScheme,new []{"bearer"} }
                };

                options.AddSecurityRequirement(securityRequirments);
            });
            return services;
        }
    }
}
