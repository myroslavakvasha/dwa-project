using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
