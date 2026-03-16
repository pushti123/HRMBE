using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Holiday
{
    public int HolidayId { get; set; }

    public string HolidayName { get; set; } = null!;

    public DateOnly HolidayDate { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }
}
