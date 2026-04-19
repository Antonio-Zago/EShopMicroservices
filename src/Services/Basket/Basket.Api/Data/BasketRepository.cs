using Basket.Api.Exceptions;
using Basket.Api.Models;
using Marten;

namespace Basket.Api.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            session.Delete<ShoppingCart>(userName);

            await session.SaveChangesAsync();

            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(userName,cancellationToken);

            return basket is null ? throw new BasketNotFoundException(userName) : basket;
        }

        //Se o shoppingCart já existir ele atualiza, se não existir insere
        public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            session.Store(shoppingCart);

            await session.SaveChangesAsync();

            return shoppingCart;
        }
    }
}
