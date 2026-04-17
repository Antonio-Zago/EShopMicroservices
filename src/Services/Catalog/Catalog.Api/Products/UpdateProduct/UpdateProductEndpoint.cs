
using Catalog.Api.Products.CreateProduct;

namespace Catalog.Api.Products.UpdateProduct
{
    public class UpdateProductEndpoint : CarterModule
    {
        public record UpdateProductRequest(Guid Guid, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

        public record UpdateProductResponse(bool isSucess);

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/", async (ISender sender, UpdateProductRequest request) =>
            {
                var command = request.Adapt<UpdateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
             .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
        }
    }
}
