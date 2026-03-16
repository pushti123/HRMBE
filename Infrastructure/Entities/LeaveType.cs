using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string LeaveName { get; set; } = null!;

    public string? LeaveCode { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDelete { get; set; }

    public bool? IsAutoAssign { get; set; }

    public virtual ICollection<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; } = new List<EmployeeLeaveBalance>();

    public virtual ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();

    public virtual ICollection<LeavePolicy> LeavePolicies { get; set; } = new List<LeavePolicy>();
}
