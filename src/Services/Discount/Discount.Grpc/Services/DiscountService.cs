using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {

        public override async Task<CupounModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(a => a.ProductName == request.ProductName);

            if (coupon is null)
            {
                coupon = new Coupon() { ProductName = "No Discount", Amount = 0, Description = "No Discount" };
            }

            var couponModel = coupon.Adapt<CupounModel>();

            return couponModel;
        }

        public override async Task<CupounModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }

            var couponSalvo = await dbContext.Coupons.AddAsync(coupon);

            await dbContext.SaveChangesAsync();

            var couponModel = coupon.Adapt<CupounModel>();

            return couponModel;
        }

        public override async Task<CupounModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }

            dbContext.Coupons.Update(coupon);

            await dbContext.SaveChangesAsync();

            var couponModel = coupon.Adapt<CupounModel>();

            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }

            dbContext.Coupons.Remove(coupon);

            await dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse() { Sucess = true};
        }
    }
}
