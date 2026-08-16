using Basket.Api.Dtos;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Basket.CheckoutBasket
{
    public record CheckoutBasketRequest(BasketCheckoutDto basketCheckoutDto);
    public record CheckoutBasketResponse(bool isSucess);
    public class CheckoutBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<CheckoutBasketCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CheckoutBasketResponse>();

                return Results.Ok(response);
            }).WithName("CheckoutBasket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("CheckoutBasket")
            .WithDescription("CheckoutBasket");
        }
    }
}
