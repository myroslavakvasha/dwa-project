namespace DAL.DTOs.Order
{
    public class OrderDetailResponseDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public string? PaymentType { get; set; }

        public string? Comment { get; set; }
        public List<OrderItemResponseDto> OrderItems { get; set; }
    }
}
