using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.User
{
    public class EditUserBankInfoRequestDTO
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
    }
}
