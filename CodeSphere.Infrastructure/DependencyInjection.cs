using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Infrastructure.Context;
using CodeSphere.Infrastructure.Implementation;
using CodeSphere.Infrastructure.Implementation.Repositories;
using CodeSphere.Infrastructure.Implementation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;


namespace CodeSphere.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services Registration

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IExecutionService), typeof(ExecutionService));
            services.AddScoped(typeof(ISubmissionRepository), typeof(SubmissionRepository));
            services.AddScoped(typeof(IProblemRepository), typeof(ProblemRepository));
            services.AddScoped(typeof(IFileService), typeof(FileService));
            services.AddScoped(typeof(IContestRepository), typeof(ContestRepository));
            services.AddScoped(typeof(IBlogRepository), typeof(BlogRepository));
            services.AddScoped(typeof(IResponseCacheService), typeof(ResponseCacheService));
            services.AddScoped(typeof(IEmailService), typeof(EmailService));
            services.AddScoped(typeof(IElasticSearchRepository), typeof(ElasticSearchRepository));
            services.AddScoped(typeof(ITopicRepository), typeof(TopicRepository));
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                //var configs = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect("127.0.0.1:6379");
            });

            //services.AddStackExchangeRedisCache(x =>
            //{
            //    //,abortConnect=false
            //    x.Configuration = "codesphere.cache:6379";
            //});

            #endregion

            #region db context
            services.AddDbContext<ApplicationDbContext>(options =>
             {
                 //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                 options.UseSqlServer(configuration.GetConnectionString("Remote"));
             });
            #endregion

            #region Identity context

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            #endregion

            #region JWT Authentication

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? string.Empty)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });


            #endregion

            return services;
        }
    }
}
