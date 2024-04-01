using System;
using System.Collections.Generic;

namespace WebApplication52.Models;

public partial class Device
{
    public int Id { get; set; }

    public byte Location { get; set; }

    public byte FixReason { get; set; }

    public string? EmpId { get; set; } 

    public byte Status { get; set; }

    public string? FixEmpId { get; set; }

    public byte Solution { get; set; }

    public string? Remark { get; set; }

}
