using NServiceBus;

namespace NSB6SelfHostedClientMVC.Messages.Commands
{
    public class TestCommand : ICommand
    {
        public string TestProperty { get; set; }
    }
}
