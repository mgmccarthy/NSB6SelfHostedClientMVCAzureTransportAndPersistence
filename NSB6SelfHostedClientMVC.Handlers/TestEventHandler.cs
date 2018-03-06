using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NSB6SelfHostedClientMVC.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace NSB6SelfHostedClientMVC.Handlers
{
    public class TestEventHandler : IHandleMessages<TestEvent>
    {
        static ILog log = LogManager.GetLogger<TestEventHandler>();

        public Task Handle(TestEvent message, IMessageHandlerContext context)
        {
            Debug.WriteLine("handled TestEvent");
            log.Info($"handled TestEvent at {DateTime.UtcNow} (UTC)");
            return Task.CompletedTask;
        }
    }
}
