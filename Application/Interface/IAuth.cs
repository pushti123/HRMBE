using Application.DTO.RequestDTO.Auth;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO;
using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Interface
{
    public interface IAuth
    {

        public Task<CommonResponse> Login(LoginRequestDTO request);
        public Task<CommonResponse> RefreshToken(RefreshTokenRequestDTO request);
        public Task<CommonResponse> RegisterUser(AddEditUserPersonalInfoRequestDTO request);

    }
}
