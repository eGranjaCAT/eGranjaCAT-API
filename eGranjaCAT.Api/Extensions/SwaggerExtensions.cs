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
                    Title = "eGranjaCAT - RESTful API for Pig Farm Management",
                    Version = "v1",
                    Description =
                    "The eGranjaCAT API is developed with .NET 8 and C#, implementing clean architecture and a single-tenant-per-database design on SQL Server to ensure data isolation and client-specific customization. " +
                    "It features secure JWT authentication with role- and policy-based access control, comprehensive Swagger documentation for API endpoints, scheduled background tasks via cron jobs, and SMTP integration for email notifications. " +
                    "The API offers native integrations with official systems such as Gestió Telemàtica Ramadera (GTR), Ministerio de Agricultura, Pesca y Alimentación (MAPA), the PresVet antibiotic prescription monitoring system, and other services. " +
                    "It also includes in-house generation and digital signing of veterinary electronic prescriptions without reliance on third-party software, with some features currently in development. Additionally, it provides automated PDF and XLSX document generation based directly on database content. " +
                    "This API streamlines pig farm management workflows in Catalonia by automating data exchange and enhancing operational security and efficiency. It is currently being used by the Catalan livestock and agriculture company Nastrafarm SL.",
                    Contact = new OpenApiContact
                    {
                        Name = "Felix Montragull Kruse",
                        Email = "fmontrakruse@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/felixmk/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "© 2025 - All rights reserved by Felix Montragull Kruse"
                    }
                });


                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                var securityRequirement = new OpenApiSecurityRequirement
                {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "bearerAuth"
                             }
                         },
                         new string[] {}
                     }
                };
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}