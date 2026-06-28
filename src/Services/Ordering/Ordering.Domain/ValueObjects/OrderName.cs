using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        private const int MaxLength = 5;

        public string Value { get; }

        public static OrderName Of(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, MaxLength);
            return new OrderName(value);
        }

        private OrderName(string value)
        {
            Value = value;
        }
    }
}
