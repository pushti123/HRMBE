using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class DepartmentMst
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }
}
