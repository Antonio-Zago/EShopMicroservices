using BuildingBlocks.CQRS;
using Catalog.Api.Models;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.Api.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Product> products);

    internal class GetProductByCategoryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                            .Where(p => p.Category.Contains(request.Category))
                            .ToListAsync();

            return new GetProductByCategoryResult(products);
        }
    }
}
