using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.id);

            var order = await applicationDbContext.Orders.FindAsync([orderId], cancellationToken);

            if (order is null)
            {
                throw new OrderNotFoundException(command.Order.id);
            }

            UpdateOrderWithNewValues(order, command.Order);

            applicationDbContext.Orders.Update(order);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
        {
            var shippingAddress = Address.Of(
                orderDto.ShippingAdress.FirstName,
                orderDto.ShippingAdress.LastName,
                orderDto.ShippingAdress.EmailAddress,
                orderDto.ShippingAdress.AddressLine,
                orderDto.ShippingAdress.Country,
                orderDto.ShippingAdress.State,
                orderDto.ShippingAdress.ZipCode);

            var billingAddress = Address.Of(
                orderDto.BillingAddress.FirstName,
                orderDto.BillingAddress.LastName,
                orderDto.BillingAddress.EmailAddress,
                orderDto.BillingAddress.AddressLine,
                orderDto.BillingAddress.Country,
                orderDto.BillingAddress.State,
                orderDto.BillingAddress.ZipCode);

            var payment = Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                int.Parse(orderDto.Payment.PaymentMethod));

            order.Update(
                orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                payment: payment,
                status: orderDto.Status);
        }
    }
}
