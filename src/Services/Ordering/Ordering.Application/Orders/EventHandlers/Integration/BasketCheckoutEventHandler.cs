using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            logger.LogInformation($"Integration Evevnt Handled: {context.Message.GetType().Name}");

            var command = MapToCreateOrderCommand(context.Message);
            await sender.Send(command);
        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            // O BasketCheckoutEvent é um evento de integração "achatado": carrega um único
            // endereço e os dados de pagamento, então usamos o mesmo endereço para entrega
            // e cobrança.
            var address = new AdressDto(
                message.FirstName,
                message.LastName,
                message.EmailAddress,
                message.AddressLine,
                message.Country,
                message.State,
                message.ZipCode);

            // PaymentMethod chega como int no evento, mas o PaymentDto/CreateOrderHandler
            // trabalham com string (o handler faz int.Parse depois).
            var payment = new PaymentDto(
                message.CardName,
                message.CardNumber,
                message.Expiration,
                message.Cvv,
                message.PaymentMethod.ToString());

            var orderId = Guid.NewGuid();

            // Limitação conhecida: o BasketCheckoutEvent não transporta a lista de itens do
            // carrinho, apenas o TotalPrice. Para satisfazer as regras de negócio (o validator
            // exige OrderItems e Order.Add exige quantidade/preço > 0) criamos um único item
            // representando o total do pedido. O ideal é evoluir o evento para carregar os itens.
            var orderItems = new List<OrderItemDto>
            {
                new OrderItemDto(orderId, Guid.NewGuid(), 1, message.TotalPrice)
            };

            var orderDto = new OrderDto(
                id: orderId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                ShippingAdress: address,
                BillingAddress: address,
                Payment: payment,
                Status: OrderStatus.Pending,
                OrderItems: orderItems);

            return new CreateOrderCommand(orderDto);
        }
    }
}
