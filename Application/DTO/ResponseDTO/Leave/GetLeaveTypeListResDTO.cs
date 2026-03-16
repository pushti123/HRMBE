using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.ResponseDTO.Leave
{
    public class GetLeaveTypeListResDTO
    {
        public int TotalCount { get; set; }

        public List<LeaveType> LeaveTypeDetail {  get; set; }
        public class LeaveType
        {
            public int LeaveTypeId { get; set; }
            public string LeaveName {  get; set; }
            public string LeaveCode { get; set; }
            public bool IsAutoAssign { get; set; }
        }
    }
}
