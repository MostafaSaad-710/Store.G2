using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Domain.Entities.Products;
using Store.G02.persistence.Data.Contexts;
using Store.G02.persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G02.persistence.Repositories
{

    // CLR 
    public class DbIntializer(StoreDbContext _context, 
        StoreIdentityDbContext identityDbContext, 
        UserManager<AppUser> userManager, 
        RoleManager<IdentityRole> roleManager) : IDbInitializer
    {
        private readonly StoreIdentityDbContext identityDbContext = identityDbContext;
        private readonly UserManager<AppUser> userManager = userManager;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;

        public async Task InitializeAsync()
        {
            // Create Db
            // Update Db 
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();
            }
            // Data Seeding

            if (!_context.ProductBrands.Any())
            {
                // ProductBrands
                // 1. Real All Data From Json File (brands.json)
                // \Infrastructure\Store.G02.persistence\Data\DataSeeding\brands.json
                var brandsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.persistence\Data\DataSeeding\brands.json");

                // 2. Convert The JsonString To List<ProductBrand>

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

                // 3. Add  List To The Db
                if (brands is not null && brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                }
            }


            // BroductTypes
            if (!_context.ProductTypes.Any())
            {
                // ProductBrands
                // 1. Real All Data From Json File (brands.json)
                // \Infrastructure\Store.G02.persistence\Data\DataSeeding\brands.json
                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.persistence\Data\DataSeeding\types.json");

                // 2. Convert The JsonString To List<ProductBrand>

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                // 3. Add  List To The Db
                if (types is not null && types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                }
            }

            // Products
            if (!_context.Products.Any())
            {
                // ProductBrands
                // 1. Real All Data From Json File (brands.json)
                // \Infrastructure\Store.G02.persistence\Data\DataSeeding\brands.json
                var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.persistence\Data\DataSeeding\products.json");

                // 2. Convert The JsonString To List<ProductBrand>

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                // 3. Add  List To The Db
                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                }
            }


            await _context.SaveChangesAsync();
        }

        public async Task InitializeIdentityAsync()
        {
            // Create Database If It dosent Exists && Apply To Any Pending Migrations
            if (identityDbContext.Database.GetPendingMigrations().Any())
            {
                await identityDbContext.Database.MigrateAsync();
            }

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });
                await roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
            }

            // Seeding
            if (!userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                };
                var adminUser = new AppUser()
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                };

                await userManager.CreateAsync(superAdminUser, "P@ssw0rd");
                await userManager.CreateAsync(adminUser, "P@ssw0rd");

                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await userManager.AddToRoleAsync(adminUser, "Admin");

            }
        }
    }
}
