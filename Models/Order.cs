namespace Advantage.API.Models
{
    public class Order
    {
        public Guid Id {get; set;}
        public Customer Customer {get; set;}
        public decimal Total {get; set;}
        public DateTime Placed {get; set;}
        public DateTime? Completed {get; set;}
        
    }   
}