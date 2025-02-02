using Bal.Helpers;
using Bal.ServiceContracts;
using Bal.Services;
using Dal.Contracts;
using Dal.Data;
using Dal.Repositories;
using Dal.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Models;
using PhysicalPersons.Filters.ActionFilters;

namespace PhysicalPersons.Configurations;

public static  class ConfigureServices
{
    public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PersonDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddControllers(options =>
            {
                options.Filters.Add(new ModelValidateAttribute());  
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });
        
  
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IFileFactory, FileFactory>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();
        services.AddScoped<IRelationshipRepository, RelationshipRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRelationshipService, RelationshipService>();
        services.AddAuthorization();
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });
    }
    public static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.AddOpenApi();
    }
}