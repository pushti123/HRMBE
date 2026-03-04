using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.User
{
    public class GetUserListRequestDTO
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool OrderBy { get; set; }
    }
}
