using BuildingBlocks.Exceptions;

namespace Catalog.Api.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid guid) : base("Product not found!", guid)
        {
            
        }
    }
}
