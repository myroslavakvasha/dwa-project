using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Log
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int LogLevelId { get; set; }

    public string Message { get; set; } = null!;

    public virtual LogLevel LogLevel { get; set; } = null!;
}
