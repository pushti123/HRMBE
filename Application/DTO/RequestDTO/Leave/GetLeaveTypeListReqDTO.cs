using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.Leave
{
    public class GetLeaveTypeListReqDTO
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool OrderBy { get; set; }

        public string? SearchString {  get; set; }

        public string LeaveTypeStatusFilter {  get; set; }
    }
}
