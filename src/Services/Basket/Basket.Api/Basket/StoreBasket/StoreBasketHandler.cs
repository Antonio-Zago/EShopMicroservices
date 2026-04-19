using Basket.Api.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

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

    public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
