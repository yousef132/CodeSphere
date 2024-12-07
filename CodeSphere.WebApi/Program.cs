using CodeSphere.Application;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Middleware;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Infrastructure;
using CodeSphere.Infrastructure.Seeder;
using CodeSphere.WebApi.Extentions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


// swagger documentation
builder.Services.AddSwaggerServices()
    .AddSwaggerDocumentation();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
});

builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();



var Cors = "_CORS";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: Cors,
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        }
    );
});


var app = builder.Build();

app.MigrationInitialization();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var elasticSearch = scope.ServiceProvider.GetRequiredService<IElasticSearchRepository>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
    await elasticSearch.InitializeIndexes();

}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerService();
}
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors(Cors);
app.UseAuthorization();

app.MapControllers();

app.Run();