using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.Leave
{
    public class AddEditLeaveTypeReqDTO
    {
        public int Id {  get; set; }
        public string LeaveName { get; set; } = null!;

        public string LeaveCode { get; set; } = null!;

        public string Description { get; set; } = null!;
        public bool IsAutoAssign { get; set; }

    }
}
