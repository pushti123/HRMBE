using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class LeavePolicy
{
    public int LeavePolicyId { get; set; }

    public int LeaveTypeId { get; set; }

    public bool? IsProbationApplicable { get; set; }

    public int? ProbationLeaveDays { get; set; }

    public string? LeaveCreditType { get; set; }

    public int? LeaveDays { get; set; }

    public bool? IsCarryForward { get; set; }

    public int? MaxCarryForwardDays { get; set; }

    public bool? IsSandwichApplicable { get; set; }

    public bool? IsHolidaySandwichApplicable { get; set; }

    public bool? IsWeelOffSandwichApplicable { get; set; }

    public int? MaxLeavePerRequest { get; set; }

    public bool? IsHalfDayAllowed { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual LeaveType LeaveType { get; set; } = null!;
}
