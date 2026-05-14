using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class LogLevel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
