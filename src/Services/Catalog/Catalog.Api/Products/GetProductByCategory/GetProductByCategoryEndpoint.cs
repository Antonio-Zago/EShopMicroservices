
using Catalog.Api.Models;
using Catalog.Api.Products.GetProductById;

namespace Catalog.Api.Products.GetProductByCategory
{
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public record GetProductByCategoryResponse(IEnumerable<Product> products);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/category/{category}", async (string category, ISender sender) =>
            {
                var products = await sender.Send(new GetProductByCategoryQuery(category));

                var response = products.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
                .WithName(nameof(GetProductByCategoryEndpoint))
                .Produces(StatusCodes.Status200OK)
                .WithDescription("Get product by Category")
                .ProducesProblem(StatusCodes.Status400BadRequest); ;
        }
    }
}
