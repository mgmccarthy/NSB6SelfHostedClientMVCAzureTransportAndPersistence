using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;
using NServiceBus.Persistence.Sql;

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

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var connection = @"Data Source=sql; Initial Catalog=NsbSqlPersistence; Integrated Security=False; User ID=sa; Password=Password123!; Connect Timeout=60; Encrypt=False;";
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));

            //SqlHelper.EnsureDatabaseExists(connection);

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
