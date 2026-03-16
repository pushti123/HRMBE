using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class EmployeeLeaveBalance
{
    public int BalanceId { get; set; }

    public int EmployeeId { get; set; }

    public int LeaveTypeId { get; set; }

    public int Year { get; set; }

    public int AllocatedDays { get; set; }

    public int? UsedDays { get; set; }

    public int RemainingDays { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual LeaveType LeaveType { get; set; } = null!;
}
