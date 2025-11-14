using Store.G02.Domain.Entities.orders;
using Store.G02.Services.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Specifications
{
    public class OrderSpecification : BaseSpecifications<Guid, Order>
    {
        public OrderSpecification(Guid id, string userEmail): base(O => O.Id == id && O.UserEmail.ToLower() == userEmail.ToLower())
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }

        public OrderSpecification(string userEmail) : base(O => O.UserEmail.ToLower() == userEmail.ToLower())
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDescending(O => O.OrderDate);
        }

    }
}
