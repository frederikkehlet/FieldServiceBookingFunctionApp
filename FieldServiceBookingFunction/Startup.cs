using Domain.Services;
using Domain.Services.Implementation;
using Domain.Handlers;
using Domain.Handlers.Implementation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using Domain.Clients.Implementation;

[assembly: FunctionsStartup(typeof(FieldServiceBookingFunction.Startup))]

namespace FieldServiceBookingFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped(provider =>
            {
                string connectionString = $@"
                    AuthType=ClientSecret;
                    Url={Environment.GetEnvironmentVariable("DataverseEnvironment")};
                    ClientId={Environment.GetEnvironmentVariable("DataverseClientId")};
                    ClientSecret={Environment.GetEnvironmentVariable("DataverseClientSecret")}";

                return new ServiceClient(connectionString);
            });

            builder.Services.AddScoped(provider => 
            {
                string scope = "https://graph.microsoft.com/.default";
                string tenantId = Environment.GetEnvironmentVariable("TenantId");
                string clientId = Environment.GetEnvironmentVariable("DataverseClientId");
                string clientSecret = Environment.GetEnvironmentVariable("DataverseClientSecret");

                return new GraphClient(tenantId, clientId, clientSecret, scope);
            });

            builder.Services.AddScoped<IBookableResourceBookingService, BookableResourceBookingService>();
            builder.Services.AddScoped<IBookableResourceBookingToOutlookEventHandler, BookableResourceBookingToOutlookEventHandler>();
            builder.Services.AddScoped<ICalendarService, CalendarService>();
            builder.Services.AddScoped<ISystemUserService, SystemUserService>();
            builder.Services.AddScoped<IBookableResourceService, BookableResourceService>();
            builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
            
            builder.Services.AddLogging();
        }
    }
}
