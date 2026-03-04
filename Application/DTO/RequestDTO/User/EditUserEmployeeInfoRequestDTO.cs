using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.User
{
    public class EditUserEmployeeInfoRequestDTO
    {
        public int Id { get; set; }
        public int DepartmentId {  get; set; }
        public int DesignationId { get; set; }
        public int RoleId {  get; set; }
    }
}
