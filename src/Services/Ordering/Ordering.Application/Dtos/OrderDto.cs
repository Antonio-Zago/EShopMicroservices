using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Dtos
{
    public record OrderDto(
        Guid id, 
        Guid CustomerId,
        string OrderName,
        AdressDto ShippingAdress,
        AdressDto BillingAddress,
        PaymentDto Payment,
        OrderStatus Status,
        List<OrderItemDto> OrderItems
    );
}
