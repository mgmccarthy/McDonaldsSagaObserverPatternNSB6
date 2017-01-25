using System.Threading;
using System.Threading.Tasks;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace McDonaldsSagaObserverPattern.MenuStationEndpoint.Handlers
{
    public class MakeFriesHandler : IHandleMessages<MakeFries>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MakeFriesHandler));
        
        public async Task Handle(MakeFries message, IMessageHandlerContext context)
        {
            Log.Warn("starting to make fries");
            Thread.Sleep(10000); //10 seconds
            Log.Warn("fries done");
            await context.Reply(new FriesCompleted { OrderId = message.OrderId });
        }
    }
}
