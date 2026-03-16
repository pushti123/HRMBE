using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.ResponseDTO.Leave
{
    public class GetLeavePolicyListResDTO
    {
        public int TotalCount { get; set; }

        public List<LeavePolicy> LeavePolicyDetail { get; set; }
        public class LeavePolicy
        {
            public int LeavePolicyId { get; set; }
            public string LeaveTypeName { get; set; }
            public string LeaveCreditType { get; set; }
            public int MaxLeavePerRequest { get; set; }
            public int LeaveDays { get; set; }
            public bool? IsProbationApplicable { get; set; }
        }
    }
}
