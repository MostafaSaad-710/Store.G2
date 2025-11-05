using AutoMapper;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Products;
using Store.G02.Domain.Exceptions;
using Store.G02.Services.Abstractions.Products;
using Store.G02.Services.Specifications;
using Store.G02.Services.Specifications.Products;
using Store.G02.Shared;
using Store.G02.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Products
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {

        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters)
        {
            //var spec = new BaseSpecifications<int, Product>(null);
            //spec.Includes.Add(P => P.Brand);
            //spec.Includes.Add(P => P.Type);

            var spec = new ProductsWithBrandAndTypeSpecifications(parameters);

            var products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync(spec);
           
            var result = _mapper.Map<IEnumerable<ProductResponse>>(products);

            var specCount = new ProductsCountSpecifications(parameters);

            var count = await _unitOfWork.GetRepository<int, Product>().CountAsync(specCount);

            return new PaginationResponse<ProductResponse>(parameters.PageIndex, parameters.PageSize, count, result);
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var spec = new ProductsWithBrandAndTypeSpecifications(id);


            var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundExceptions(id);
            var result = _mapper.Map<ProductResponse>(product);
            return result;
        }

        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(brands);
            return result;
        }

        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypeAsync()
        {
            var types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTypeResponse>>(types);
            return result;
        }

        
    }
}
