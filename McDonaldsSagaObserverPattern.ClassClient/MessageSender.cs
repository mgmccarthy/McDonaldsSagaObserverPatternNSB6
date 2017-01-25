using System;
using System.Threading.Tasks;
using McDonaldsSagaObserverPattern.Messages;
using McDonaldsSagaObserverPattern.Messages.Commands;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.ClassClient
{
    public class MessageSender : IWantToRunWhenEndpointStartsAndStops
    {
        public Task Start(IMessageSession session)
        {
            Console.WriteLine("Press 'Enter' to place an Order.To exit, Ctrl + C");
            while (Console.ReadLine() != null)
            {
                var placeOrder = new PlaceOrder { OrderId = Guid.NewGuid(), Fries = new Fries(), Shake = new Shake() };
                return session.Send(placeOrder);
            }
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}
