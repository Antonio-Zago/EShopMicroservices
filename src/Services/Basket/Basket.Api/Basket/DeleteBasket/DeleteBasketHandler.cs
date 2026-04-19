
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

    public class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
