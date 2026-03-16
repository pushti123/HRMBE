using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class LeaveApplicationDetail
{
    public int LeaveDetailId { get; set; }

    public int LeaveApplicationId { get; set; }

    public DateOnly LeaveDate { get; set; }

    public string LeaveDayType { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual LeaveApplication LeaveApplication { get; set; } = null!;
}
