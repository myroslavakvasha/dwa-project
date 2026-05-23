using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int FoodId { get; set; }

    public decimal TotalPrice { get; set; }

    public int Quantity { get; set; }

    public virtual Food Food { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
