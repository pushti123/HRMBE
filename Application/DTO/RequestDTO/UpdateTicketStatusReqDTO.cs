using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO
{
    public class UpdateTicketStatusReqDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
    }
}
