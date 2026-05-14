using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public int PaymentId { get; set; }

    public string? Comment { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment Payment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
