using System.Threading;
using System.Threading.Tasks;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace McDonaldsSagaObserverPattern.MenuStationEndpoint.Handlers
{
    public class MakeShakeHandler : IHandleMessages<MakeShake>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MakeShakeHandler));

        public async Task Handle(MakeShake message, IMessageHandlerContext context)
        {
            Log.Warn("starting to make shake");
            Thread.Sleep(3000); //3 seconds
            Log.Warn("shake done");
            await context.Reply(new ShakeCompleted { OrderId = message.OrderId });
        }
    }
}
