using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO
{
    public class AddTicketReqDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
