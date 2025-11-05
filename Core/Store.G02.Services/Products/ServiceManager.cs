using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Abstractions.Products;
using Store.G02.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Products
{
    public class ServiceManager(IUnitOfWork _unitOfWork, 
        IMapper _mapper, 
        IBasketRepository _basketRepository, 
        ICacheRepository _cacheRepository, 
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork, _mapper);
        public IBasketService BasketService { get; } = new BasketService(_basketRepository, _mapper);
        public ICacheService CacheService { get; } = new CacheService(_cacheRepository);
        public IAuthService AuthService { get; } = new AuthService(userManager, options);
    }
}
