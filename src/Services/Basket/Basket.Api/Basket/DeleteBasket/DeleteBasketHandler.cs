
using Basket.Api.Data;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.Api.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string userName) : ICommand<DeleteBasketResult>;

    public record DeleteBasketResult(bool isSucess);

    public class DeleteBasketValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketValidator()
        {
            RuleFor(a => a.userName).NotEmpty().WithMessage("userName can't be null");
        }
    }

    public class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.DeleteBasket(request.userName, cancellationToken);

            return new DeleteBasketResult(result);
        }
    }
}
