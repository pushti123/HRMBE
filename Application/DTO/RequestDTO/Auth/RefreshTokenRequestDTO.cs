using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.Auth
{
    public class RefreshTokenRequestDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
