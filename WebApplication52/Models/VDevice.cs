using System;
using System.Collections.Generic;

namespace WebApplication52.Models;

public partial class VDevice
{
    public string? 設備位置 { get; set; }

    public string? 報修原因 { get; set; }

    public string 報修人員工號 { get; set; } = null!;

    public string? 維修人員工號 { get; set; }

    public string? 維修進度 { get; set; }

    public string? 問題原因 { get; set; }

    public int Id { get; set; }

    public string? 備註 { get; set; }

    public DateTime 日期 { get; set; }
}
