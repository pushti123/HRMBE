using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.ResponseDTO.Auth
{
    public class RefreshTokenResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
