using Basket.Api.Basket.StoreBasket;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Basket.DeleteBasket
{
    public record DeleteBasketResponse(bool isSucess);

    public class DeleteBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response); 
            })
            .WithName("DeleteProducts")
            .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Products")
            .WithDescription("Delete Products");
        }
    }
}
