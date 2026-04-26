using Basket.Api.Data;
using Basket.Api.Exceptions;
using Basket.Api.Models;
using BuildingBlocks.CQRS;
using Mapster;

namespace Basket.Api.Basket.GetBasket
{
    public record GetBasketQuery(string userName) :IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart shoppingCart);

    public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var shoppingCart = await repository.GetBasket(query.userName, cancellationToken);

            return new GetBasketResult(shoppingCart);
        }
    }
}
