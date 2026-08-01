using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.OrderId);

            var order = await applicationDbContext.Orders.FindAsync([orderId], cancellationToken);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            applicationDbContext.Orders.Remove(order);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);
        }
    }
}
