namespace NSB6SelfHostedClientMVC.Handlers
{
    using NServiceBus;
    using NServiceBus.Persistence.Sql;
    using System;
    using System.Data.SqlClient;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            const string connectionString = "UseDevelopmentStorage=true";

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

            SqlHelper.EnsureDatabaseExists(connection);

            var transportSettings = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transportSettings.ConnectionString(connectionString);
            transportSettings.DelayedDelivery().DisableTimeoutManager();
            var delayedDelivery = transportSettings.DelayedDelivery();
            delayedDelivery.UseTableName("delaysNSB6SelfHostedClientMVCHandlers");
            
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
        }
    }
}
