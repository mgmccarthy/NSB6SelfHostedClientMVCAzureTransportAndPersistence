using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;

namespace NSB6SelfHostedClientMVC.Configuration
{
    public class ConfigureRouting : INeedInitialization
    {
        public void Customize(EndpointConfiguration configuration)
        {
            //var settings = configuration.UseTransport<MsmqTransport>().Routing();
            
            ////this is how to "send a command"
            //settings.RouteToEndpoint(typeof(NSB6SelfHostedClientMVC.Messages.Commands.TestCommand), "NSB6SelfHostedClientMVC.Handlers");

            ////this is how to "subscribe to an event" (although you would never do that from a send-only endpoint like this file is in right now.
            ////settings.RegisterPublisher(typeof(IXRS.Accountability.Accountability.Events.AccountabilityInformationRemoved), "IXRS.Services.Developer");
        }
    }
}