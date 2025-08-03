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
                    Title = "eGranjaCAT - API RESTful per a la Gestió d'Explotacions Porcines",
                    Version = "v1",
                    Description =
                    "API oficial desenvolupada per Nastrafarm S.L. amb l'objectiu de millorar i automatitzar la gestió d'explotacions porcines catalanes. " +
                    "Aquest servei integra funcionalitats avançades com la connexió amb la Gestió Telemàtica Ramadera (GTR), serveis del Ministeri d'Agricultura, " +
                    "generació de documents PDF i XLSX, gestió d'autenticacions i potencialment la signatura electrònica de prescripcions i registres veterinaris. " +
                    "L'API facilita la digitalització i eficiència en el maneig de dades, reduint errors i millorant la presa de decisions a les granges.",
                    Contact = new OpenApiContact
                    {
                        Name = "Felix Montragull Kruse",
                        Email = "fmontrakruse@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/felixmk/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "© 2025 - Tots els drets reservats a Felix Montragull Kruse"
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
