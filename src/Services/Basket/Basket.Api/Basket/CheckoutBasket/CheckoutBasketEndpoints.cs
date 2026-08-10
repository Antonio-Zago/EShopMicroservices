using Basket.Api.Basket.GetBasket;
using Basket.Api.Dtos;
using Basket.Api.Models;
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
            //app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
            //{
            //    var commad = request.Adapt<CheckoutBasketCommand>();

            //    var result = await sender.Send(commad);

            //    var response = result.Adapt<CheckoutBasketResponse>();

            //    return Results.Ok(response);
            //}).WithName("CheckoutBasket")
            //.Produces<GetBasketResponse>(StatusCodes.Status201Created)
            //.ProducesProblem(StatusCodes.Status400BadRequest)
            //.WithSummary("CheckoutBasket")
            //.WithDescription("CheckoutBasket");
        }
    }
}
