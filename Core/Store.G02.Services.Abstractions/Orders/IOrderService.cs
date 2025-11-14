using Store.G02.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Abstractions.Orders
{
    public interface IOrderService
    { 
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request , string userEmail);
        Task<DeliveryMethodResponse> GetAllDeliveryMethodAsync();
        Task<OrderResponse> GetOrderByIdForSpacificUserAsync(Guid id ,  string UserEmail);
        Task<IEnumerable<OrderResponse>> GetOrdersByIdForSpacificUserAsync( string UserEmail);

    }
}
