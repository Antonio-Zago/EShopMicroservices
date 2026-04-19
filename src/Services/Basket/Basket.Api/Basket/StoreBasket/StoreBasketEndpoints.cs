using Basket.Api.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart shoppingCart);

    public record StoreBasketResponse(string userName);
    public class StoreBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) => {

                var command = request.Adapt<StoreBasketCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<StoreBasketResponse>();

                return Results.Created($"/basket/{response.userName}", response);

            })
            .WithName("PostProducts")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Post Products")
            .WithDescription("Post Products");
        }
    }
}
