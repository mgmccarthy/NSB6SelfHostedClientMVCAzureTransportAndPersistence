using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NSB6SelfHostedClientMVC.Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace NSB6SelfHostedClientMVC.Handlers
{
    public class TestCommandHandler : IHandleMessages<TestCommand>
    {
        static ILog log = LogManager.GetLogger<TestCommandHandler>();

        public Task Handle(TestCommand message, IMessageHandlerContext context)
        {
            Debug.WriteLine("handled TestCommand");
            log.Info($"handled TestCommand at {DateTime.UtcNow} (UTC)");
            return Task.CompletedTask;
        }
    }
}
