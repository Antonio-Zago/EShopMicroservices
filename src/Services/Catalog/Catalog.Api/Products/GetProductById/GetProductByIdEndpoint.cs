
using Catalog.Api.Models;

namespace Catalog.Api.Products.GetProductById
{
    public record GetProductByIdResponse(Product product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/product/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));

                var response = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);
            })
                .WithName(nameof(GetProductByIdEndpoint))
                .Produces(StatusCodes.Status200OK)
                .WithDescription("Get product by Id")
                .ProducesProblem(StatusCodes.Status400BadRequest);

        }
    }
}
