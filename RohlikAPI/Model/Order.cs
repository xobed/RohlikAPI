using System;

namespace RohlikAPI.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string PaymentMethod { get; set; }
    }
}