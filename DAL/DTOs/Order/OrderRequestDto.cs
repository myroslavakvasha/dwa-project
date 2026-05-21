namespace DAL.DTOs.Order
{
    public class OrderRequestDto
    {
        public string? PaymentType { get; set; }

        public string? Comment { get; set; }
        public List<OrderItemRequestDto> OrderItems { get; set; }
    }
}
