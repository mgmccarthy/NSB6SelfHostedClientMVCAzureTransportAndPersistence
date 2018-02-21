namespace NSB6SelfHostedClientMVC.Handlers
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            const string connectionString = "UseDevelopmentStorage=true";

            var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
            persistence.ConnectionString(connectionString);

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
