using Basket.Api.Data;
using Basket.Api.Dtos;
using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using Mapster;
using MassTransit;

namespace Basket.Api.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto basketCheckoutDto) : ICommand<CheckoutBasketResult>;

    public record CheckoutBasketResult(bool isSucess);

    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.basketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.basketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            // Recupera o carrinho existente com o total calculado
            var basket = await repository.GetBasket(command.basketCheckoutDto.UserName, cancellationToken);
            if (basket is null)
            {
                return new CheckoutBasketResult(false);
            }

            // Monta o evento de integração e define o valor total a partir do carrinho
            var eventMessage = command.basketCheckoutDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            // Publica o evento na fila para que o serviço de Ordering processe o pedido
            await publishEndpoint.Publish(eventMessage, cancellationToken);

            // Remove o carrinho após o checkout
            await repository.DeleteBasket(command.basketCheckoutDto.UserName, cancellationToken);

            return new CheckoutBasketResult(true);
        }
    }
}
