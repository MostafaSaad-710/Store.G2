using Store.G02.Domain.Entities;
using Store.G02.Domain.Entities.Products;
using Store.G02.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Specifications.Products
{
    public class ProductsWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductsWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyInclude();
        }

        public ProductsWithBrandAndTypeSpecifications(ProductQueryParameters parameters) : base
            (
                P =>
                (!parameters.BrandId.HasValue || P.BrandId == parameters.BrandId)
                &&
                (!parameters.TypeId.HasValue || P.TypeId == parameters.TypeId)
                &&
                (string.IsNullOrEmpty(parameters.Search) || P.Name.ToLower().Contains(parameters.Search.ToLower()))

            )
        {
            // pageIndex = 3;
            // pageSize = 5;
            // Skip : 2 * 5 (pageIndex -1) * pageSize
            // Take : 5

            ApplyPagination(parameters.PageSize, parameters.PageIndex);
            ApplySorting(parameters.Sort);
            ApplyInclude();
        }

        private void ApplySorting(string? sort)
        {
            // priceasc
            // pricedesc
            // nameasc
            if (!string.IsNullOrEmpty(sort))
            {
                // Check Value
                switch (sort.ToLower())
                {
                    case "priceasc":
                        // OrderBy = P => P.Price;
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        // OrderByDescending = P => P.Price;
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        // OrderBy = P => P.Name;
                        AddOrderBy(P => P.Name);
                        break;
                        
                }
            }
            else
            {
                //OrderBy = P => P.Name
                AddOrderBy(P => P.Name);
            }

            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
