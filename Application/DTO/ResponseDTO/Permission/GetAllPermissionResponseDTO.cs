using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.ResponseDTO.Permission
{
    public class GetAllPermissionResponseDTO
    {
        public int TotalCount { get; set; }

        public List<Role> Roles { get; set; }

        public List<PermissionDetail> Permissions { get; set; }

        // 👇 Simple class, NOT a DTO file
        public class Role
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
        }

        public class PermissionDetail
        {
            public int PermissionId { get; set; }
            public string PermissionName { get; set; }

            // RoleId → HasPermission
            public Dictionary<int, bool> RolePermissions { get; set; }
        }
    }
}
