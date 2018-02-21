using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

namespace NSB6SelfHostedClientMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IEndpointInstance endpoint;

        protected void Application_Start()
        {
            const string connectionString = "UseDevelopmentStorage=true";

            var endpointConfiguration = new EndpointConfiguration("NSB6SelfHostedClientMVCAzureTransportAndPersistence");
            endpointConfiguration.SendOnly();

            endpointConfiguration.UsePersistence<AzureStoragePersistence>().ConnectionString(connectionString);

            var transportSettings = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transportSettings.ConnectionString(connectionString);
            transportSettings.DelayedDelivery().DisableTimeoutManager();
            var delayedDelivery = transportSettings.DelayedDelivery();
            delayedDelivery.UseTableName("delaysNSB6SelfHostedClientMVC");
            

            var routingSettings = transportSettings.Routing();
            routingSettings.RouteToEndpoint(typeof(NSB6SelfHostedClientMVC.Messages.Commands.TestCommand), "NSB6SelfHostedClientMVC.Handlers");

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var mvcContainerBuilder = new ContainerBuilder();
            mvcContainerBuilder.RegisterInstance(endpoint);
            mvcContainerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
            var mvcContainer = mvcContainerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(mvcContainer));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            endpoint?.Stop().GetAwaiter().GetResult();
        }
    }
}
