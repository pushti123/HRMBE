using Api.Model.RequestModel.Auth;
using Api.Model.RequestModel.User;
using Api.Model.ResponseModel.Auth;
using Application.DTO.RequestDTO.Auth;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.Auth;
using Application.Interface;
using Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuth _iAuth;
        public AuthController(IAuth iAuth)
        {
            _iAuth = iAuth;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<CommonResponse> Login(LoginRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iAuth.Login(request.Adapt<LoginRequestDTO>());
                LoginResponseDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<LoginResponseModel>();
            }
            catch (Exception ex) { }
            return response;
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<CommonResponse> RefreshToken(RefreshTokenRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iAuth.RefreshToken(request.Adapt<RefreshTokenRequestDTO>());
                RefreshTokenResponseDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<LoginResponseModel>();
            }
            catch { throw; }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<CommonResponse> RegisterUser([FromForm] AddEditUserPersonalInfoRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iAuth.RegisterUser(request.Adapt<AddEditUserPersonalInfoRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }
    }
}

