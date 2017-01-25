
namespace McDonaldsSagaObserverPattern.ClassClient
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.SendOnly();
        }
    }
}
