using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Orders.LongCountAsync(cancellationToken);

            var orders = await context.Orders
                //.Include(o => o.OrderItems)
                .AsNoTracking()
                .OrderBy(o => o.OrderName.Value)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetOrdersResult(
                new PaginatedResult<OrderDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    orders.ToOrderDtoList()));
        }
    }
}
