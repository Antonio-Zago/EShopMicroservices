using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = CreateNweOrder(command.Order);

            applicationDbContext.Orders.Add(order);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

           return new CreateOrderResult(order.Id.Value);
        }

        private Order CreateNweOrder(OrderDto order)
        {
            var shippingAddress = Address.Of(
                order.ShippingAdress.FirstName,
                order.ShippingAdress.LastName,
                order.ShippingAdress.EmailAddress,
                order.ShippingAdress.AddressLine,
                order.ShippingAdress.Country,
                order.ShippingAdress.State,
                order.ShippingAdress.ZipCode);

            var billingAddress = Address.Of(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode);

            var newOrder = Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(order.CustomerId),
                orderName: OrderName.Of(order.OrderName),
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                payment: Payment.Of(
                    order.Payment.CardName,
                    order.Payment.CardNumber,
                    order.Payment.Expiration,
                    order.Payment.Cvv,
                    int.Parse(order.Payment.PaymentMethod)));

            foreach (var orderItem in order.OrderItems)
            {
                newOrder.Add(
                    ProductId.Of(orderItem.ProductId),
                    orderItem.Quantity,
                    orderItem.Price);
            }

            return newOrder;
        }
    }
}
