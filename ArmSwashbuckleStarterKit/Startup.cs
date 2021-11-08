using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ArmSwashbuckleStarterKit.Swagger;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ArmSwashbuckleStarterKit.Middlewares;

namespace ArmSwashbuckleStarterKit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Filters.Clear();
                options.Filters.Add(new ConsumesAttribute("application/json")); // Force all controllers to only consume application/json, as requested by ARM
                options.Filters.Add(new ProducesAttribute("application/json")); // Force all controllers to only return application/json, as requested by ARM
            });
            //.AddNewtonsoftJson();  TODO: uncomment this line if using Newtonsoft.Json

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // TODO: update OpenApiInfo with your own details
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "2021-08-11-preview",
                    Title = "Microsoft.ArmSwashbuckleStarterKit",
                    Description = "An Azure Resource Manager resource provider providing weather forecasts",
                    Contact = new OpenApiContact
                    {
                        Name = "Jim Flowers",
                        Email = "scatteredshowers@microsoft.com",
                        Url = new Uri("https://eng.ms/weather"),
                    },
                });

                c.EnableAnnotations();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.SchemaFilter<ExcludeSchemaFilter>();
                c.SchemaFilter<ReadOnlySchemaFilter>();
                c.SchemaFilter<RemoveInvalidFormatSchemaFilter>();
                c.SchemaFilter<TrackedAzureResourceSchemaFilter>();
                c.SchemaFilter<XmsEnumSchemaFilter>();
                c.SchemaFilter<XmsExtensionsSchemaFilter>();

                c.OperationFilter<DefaultResponseOperationFilter>();
                c.OperationFilter<PageableOperationFilter>();
                c.OperationFilter<RequireRequestBodyOperationFilter>();
                c.OperationFilter<ResponseDescriptionsOperationFilter>();
                c.OperationFilter<XmsExamplesOperationFilter>();
                c.OperationFilter<XmsLongRunningOperationFilter>();

                c.DocumentFilter<ExternalizeDocumentFilter>();

                c.CustomOperationIds(e =>
                {
                    var prefix = e.ActionDescriptor.RouteValues["controller"];
                    var routeAction = e.ActionDescriptor.RouteValues["action"];

                    var suffix = string.Empty;

                    if (routeAction.StartsWith("Put"))
                    {
                        suffix = "Create";
                    }
                    else if (routeAction.StartsWith("Patch"))
                    {
                        suffix = "Update";
                    }
                    else if (routeAction.StartsWith("Get"))
                    {
                        suffix = "Get";
                    }
                    else if (routeAction.StartsWith("Delete"))
                    {
                        suffix = "Delete";
                    }
                    else
                    {
                        suffix = routeAction;
                    }

                    foreach (var p in e.SupportedResponseTypes)
                    {
                        if (p.Type.Name.Contains("ResourceListResultModel"))
                        {
                            suffix = "List";
                            break;
                        }
                    }

                    return $"{prefix}_{suffix}";
                });

                // adds security definition required by ARM
                c.AddSecurityDefinition("azure_auth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("https://login.microsoftonline.com/common/oauth2/authorize"),
                            Scopes = new Dictionary<string, string> { ["user_impersonation"] = "impersonate your user account" }
                        }
                    },
                    Description = "Azure Active Directory OAuth2 Flow"
                });
            });

            // TODO: uncomment the next line if using Newtonsoft.Json for serialization
            // services.AddSwaggerGenNewtonsoftSupport(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArmSwashbuckleStarterKit v1"));
            }

            app.UseHttpsRedirection();

            // Place ExceptionHandler before Authentication and Authorization
            // to handle exceptions for auth as well
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
