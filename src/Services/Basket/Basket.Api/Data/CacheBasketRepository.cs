using Basket.Api.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data
{
    public class CacheBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await basketRepository.DeleteBasket(userName);

            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cacheBasket = await cache.GetStringAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(cacheBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cacheBasket);
            }

            var basket = await basketRepository.GetBasket(userName,cancellationToken);

            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            await basketRepository.StoreBasket(shoppingCart, cancellationToken);

            await cache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart), cancellationToken);

            return shoppingCart;
        }
    }
}
