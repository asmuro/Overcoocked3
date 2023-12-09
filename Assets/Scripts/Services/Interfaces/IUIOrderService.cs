using Assets.Scripts.Objects.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interfaces
{
    internal interface IUIOrderService
    {
        void RefreshOrders(List<IOrder> orders);
    }
}
