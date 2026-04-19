using Basket.Api.Models;
using BuildingBlocks.CQRS;

namespace Basket.Api.Basket.GetBasket
{
    public record GetBasketQuery(string userName) :IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart shoppingCart);

    public class GetBasketHandler() : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
