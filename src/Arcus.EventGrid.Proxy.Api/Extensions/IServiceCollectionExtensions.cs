﻿using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Arcus.EventGrid.Proxy.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Use OpenAPI specification
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        public static void UseOpenApiSpecifications(this IServiceCollection services)
        {
            var openApiInformation = new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Name = "Arcus Event Grid Proxy",
                    Url = new Uri("https://github.com/arcus-azure/arcus.eventgrid.proxy")
                },
                Title = "Arcus Event Grid Proxy",
                Description = "Push events to Azure Event Grid in a breeze - API allows you to send events over REST without having to worry about how Azure Event Grid works.",
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://github.com/arcus-azure/arcus.eventgrid.proxy/blob/master/LICENSE")
                }
            };

            var xmlDocumentationPath = GetXmlDocumentationPath(services);

            services.AddSwaggerGen(swaggerGenerationOptions =>
            {
                swaggerGenerationOptions.EnableAnnotations();
                swaggerGenerationOptions.SwaggerDoc("v1", openApiInformation);

                if (string.IsNullOrEmpty(xmlDocumentationPath) == false)
                {
                    swaggerGenerationOptions.IncludeXmlComments(xmlDocumentationPath);
                }
            });
        }

        private static string GetXmlDocumentationPath(IServiceCollection services)
        {
            var hostingEnvironment = services.FirstOrDefault(service => service.ServiceType == typeof(IHostingEnvironment));
            if (hostingEnvironment == null)
            {
                return string.Empty;
            }

            var contentRootPath = ((IHostingEnvironment)hostingEnvironment.ImplementationInstance).ContentRootPath;
            var xmlDocumentationPath = $"{contentRootPath}/Open-Api-Docs.xml";

            return File.Exists(xmlDocumentationPath) ? xmlDocumentationPath : string.Empty;
        }
    }
}
