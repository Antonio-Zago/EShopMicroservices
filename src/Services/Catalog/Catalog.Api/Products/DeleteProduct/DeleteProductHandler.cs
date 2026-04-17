using BuildingBlocks.CQRS;
using Catalog.Api.Exceptions;
using Catalog.Api.Models;
using JasperFx.Events.Daemon;
using Marten;

namespace Catalog.Api.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid guid) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool isSucess);

    internal class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            session.Delete<Product>(command.guid);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
