using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NSB6SelfHostedClientMVC.Messages.Commands;
using NSB6SelfHostedClientMVC.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace NSB6SelfHostedClientMVC.Handlers
{
    public class TestCommandHandler : IHandleMessages<TestCommand>
    {
        static ILog log = LogManager.GetLogger<TestCommandHandler>();

        public async Task Handle(TestCommand message, IMessageHandlerContext context)
        {
            Debug.WriteLine("handled TestCommand");
            log.Info($"handled TestCommand at {DateTime.UtcNow} (UTC)");

            await context.Publish(new TestEvent {TestProperty = "TestEvent" });
        }
    }
}
