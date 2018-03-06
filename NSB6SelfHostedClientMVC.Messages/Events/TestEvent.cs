using NServiceBus;

namespace NSB6SelfHostedClientMVC.Messages.Events
{
    public class TestEvent : IEvent
    {
        public string TestProperty { get; set; }
    }
}
