using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.Leave
{
    public class UpdateLeaveApplicationStatusRequestModel
    {
        public int LeaveApplicationId { get; set; }
        public string? Status { get; set; }
    }
}
