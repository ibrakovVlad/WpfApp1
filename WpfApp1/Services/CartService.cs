using Market.Models;
using System.Collections.Generic;
using System.Linq;

namespace Market.Services
{
    public static class CartService
    {
        public static IReadOnlyList<BoughtProduct> Products => MyDbContext.GetContext()
            .BoughtProducts.Where(x => x.User == UserService.User)
            .ToList();

        public static void Add(BoughtProduct boughtProduct)
        {
            var dbContext = MyDbContext.GetContext();

            foreach (var product in Products)
            {
                if (product.ProductId == boughtProduct.ProductId && product.Size == boughtProduct.Size)
                {
                    product.Count += boughtProduct.Count;
                    dbContext.SaveChanges();
                    return;
                }
            }

            dbContext.BoughtProducts.Add(boughtProduct);
            dbContext.SaveChanges();
        }

        public static void Clear()
        {
            var dbContext = MyDbContext.GetContext();
            dbContext.RemoveRange(Products);
            dbContext.SaveChanges();
        }
    }
}
