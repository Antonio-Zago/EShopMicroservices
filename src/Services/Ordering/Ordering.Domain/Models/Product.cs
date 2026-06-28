using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class Product : Entity<ProductId>
    {
        public static Product Create(ProductId id, string name, decimal price)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

            return new Product
            {
                Id = id,
                Name = name,
                Price = price
            };
        }

        public string Name { get; private set; } = default!;

        public decimal Price { get; private set; } = default!;
    }
}
