using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(swgr =>
        {
            swgr.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Enterwell Quiz App",
                Version = "v1",
                Description =  "**Note: Enter your token without 'Bearer ' prefix from erwsponse in Authorize prompt**"
            });
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swgr.IncludeXmlComments(xmlPath);

            swgr.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            });

            swgr.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}