using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;
using FluentValidation;
using Mapster;

namespace Basket.Api.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart shoppingCart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string userName);

    public class StoreBasketValidator : AbstractValidator<StoreBasketCommand> 
    {
        public StoreBasketValidator()
        {
            RuleFor(x => x.shoppingCart).NotNull().WithMessage("Cart can't be null");
            RuleFor(x => x.shoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {

            foreach (var item in command.shoppingCart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest() { ProductName = item.ProductName}, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }

            await repository.StoreBasket(command.shoppingCart, cancellationToken);

            var result = command.shoppingCart.Adapt<StoreBasketResult>();

            return result;
        }
    }
}
