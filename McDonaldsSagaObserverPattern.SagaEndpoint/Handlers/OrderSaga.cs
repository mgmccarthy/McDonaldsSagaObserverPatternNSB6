using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace McDonaldsSagaObserverPattern.SagaEndpoint.Handlers
{
    public class OrderSaga : Saga<OrderSaga.SagaData>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<FriesCompleted>,
        IHandleMessages<ShakeCompleted>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderSaga));

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<PlaceOrder>(m => m.OrderId).ToSaga(data => data.OrderId);
            mapper.ConfigureMapping<FriesCompleted>(m => m.OrderId).ToSaga(data => data.OrderId);
            mapper.ConfigureMapping<ShakeCompleted>(m => m.OrderId).ToSaga(data => data.OrderId);
        }

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            Log.Warn("order placed.");
            Data.OrderId = message.OrderId;

            if (message.Fries != null)
            {
                AddMenuItemToOrderList("Fries");
                await context.Send(new MakeFries { OrderId = message.OrderId, Fries = message.Fries });
            }

            if (message.Shake != null)
            {
                AddMenuItemToOrderList("Shake");
                await context.Send(new MakeShake { OrderId = message.OrderId, Shake = message.Shake });
            }

            Log.Warn("order sent to all pertinenet stations.");
        }

        public async Task Handle(FriesCompleted message, IMessageHandlerContext context)
        {
            Log.Warn("handling FriesCompleted");
            await UpdateMenuItemInOrderListToTrue("Fries", context);
        }

        public async Task Handle(ShakeCompleted message, IMessageHandlerContext context)
        {
            Log.Warn("handling ShakeCompleted");
            await UpdateMenuItemInOrderListToTrue("Shake", context);
        }

        private void AddMenuItemToOrderList(string menuItem)
        {
            Data.OrderList.Add(menuItem);
        }

        private async Task UpdateMenuItemInOrderListToTrue(string menuItem, IMessageHandlerContext context)
        {
            Log.Warn($"updating menu item in order list for {menuItem}");
            Data.OrderList.Remove(menuItem);
            if (SagaIsDone())
                await PublishOrderFinishedAndMarkSagaAsComplete(context);
        }

        private bool SagaIsDone()
        {
            return !Data.OrderList.Any();
        }

        private async Task PublishOrderFinishedAndMarkSagaAsComplete(IMessageHandlerContext context)
        {
            Log.Warn("publishing OrderCompleted");
            await context.Publish(new OrderCompleted { OrderId = Data.OrderId });
            Log.Warn("marking Saga as complete");
            MarkAsComplete();
        }
        
        public class SagaData : ContainSagaData
        {
            public Guid OrderId { get; set; }
            public List<string> OrderList { get; set; }
            
            public SagaData()
            {
                OrderList = new List<string>();
            }
        }
    }
}