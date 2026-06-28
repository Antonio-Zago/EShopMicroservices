using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; } = default!;

        public string CardNumber { get; } = default!;

        public string Expiration { get; } = default!;

        public string CVV { get; } = default!;

        public int PaymentMethod { get; } = default!;

        public static Payment Of(string? cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(cardNumber);
            ArgumentNullException.ThrowIfNull(expiration);
            ArgumentNullException.ThrowIfNull(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }

        private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }
        protected Payment() { }
    }
}
