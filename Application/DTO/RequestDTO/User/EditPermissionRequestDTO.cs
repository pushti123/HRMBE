using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.User
{
    public class EditPermissionRequestDTO
    {
        public int PermissionId { get; set; }
        public List<int> RoleId { get; set; }
    }
}
