using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.orders;
using Store.G02.Domain.Entities.Products;
using Store.G02.Domain.Exceptions;
using Store.G02.Services.Abstractions.Orders;
using Store.G02.Services.Specifications;
using Store.G02.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork , IMapper _mapper , IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {

            // 1. Get Order Address
            var orderAddress = _mapper.Map<OrderAddress>(request.ShipToAddress);

            // 2. Get Delivrymethod by id
            var delivryMethod =await _unitOfWork.GetRepository<int , DeliveryMethod>().GetAsync(request.DeliveryMethodId);
            if (delivryMethod is null) throw new DeliveryMethodNotFoundException(request.DeliveryMethodId);


            // 3. Get Order Items

            // 3.1 Get Basket by id

            var basket = await _basketRepository.GetBasketAsync(request.BasketId);
            if(basket is null) throw new BasketNotFoundException(request.BasketId);

            // 3. Convert Every Basket Item to Order Item

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                // Check Price
                // Get Product From DB
                var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundExceptions(item.Id);

                if(product.Price != item.Price) item.Price = product.Price;

                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // 4. Calculate subtotal
            var subTotal = orderItems.Sum(OI => OI.Price *  OI.Quantity);

            var order = new Order(userEmail , orderAddress , delivryMethod, orderItems, subTotal);



            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            var count = await  _unitOfWork.SaveChangesAsync();
            if(count <= 0) throw new CreateOrderBadRequestException();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<DeliveryMethodResponse> GetAllDeliveryMethodAsync()
        {
             var deliveryMethods = await _unitOfWork.GetRepository<int , DeliveryMethod>().GetAllAsync();
            return (DeliveryMethodResponse)_mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods); 
        }

        public async Task<OrderResponse> GetOrderByIdForSpacificUserAsync(Guid id, string UserEmail)
        {
             var spec = new OrderSpecification(UserEmail);
            var order = await _unitOfWork.GetRepository<Guid , Order>().GetAsync(spec);
            return (OrderResponse)_mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByIdForSpacificUserAsync(string UserEmail)
        {
            var spec = new OrderSpecification(UserEmail);
            var order = await _unitOfWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<OrderResponse>>(order);
        }
    }
}
