using Microsoft.OpenApi.Models;

namespace eGranjaCAT.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eGranjaCAT - RESTful API",
                    Version = "v1",
                    Description = "Documentació oficial de la API per la gestió d'explotacions porcines catalanes.",
                    Contact = new OpenApiContact
                    {
                        Name = "Felix Montragull Kruse",
                        Email = "fmontrakruse@gmail.com"
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
