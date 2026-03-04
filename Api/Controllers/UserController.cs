using Api.Model.RequestModel.User;
using Api.Model.ResponseModel.User;
using Api.Permission;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.User;
using Application.Interface;
using Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUser _iUser;
        public UserController(IUser iUser)
        {
           _iUser = iUser;
        }

        [HttpPost("AddEditUserPersonalInfo")]
        [HasPermission("EditUser", "AddUser")]
        public async Task<CommonResponse> AddEditUserPersonalInfo([FromForm]AddEditUserPersonalInfoRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.AddEditUserPersonalInfo(request.Adapt<AddEditUserPersonalInfoRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpPut("EditUserEmployeeInfo")]
        [HasPermission("EditUser", "AddUser")]
        public async Task<CommonResponse> EditUserEmployeeInfo(EditUserEmployeeInfoRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.EditUserEmployeeInfo(request.Adapt<EditUserEmployeeInfoRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpPut("EditUserBankInfo")]
        [HasPermission("EditUser", "AddUser")]
        public async Task<CommonResponse> EditUserBankInfo(EditUserBankInfoRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.EditUserBankInfo(request.Adapt<EditUserBankInfoRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetUserId")]
        [AllowAnonymous]
        public async Task<CommonResponse> GetUserId()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetUserId();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetRole")]
        public async Task<CommonResponse> GetRole()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetRole();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetDepartment")]
        public async Task<CommonResponse> GetDepartment()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetDepartment();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetDesignation")]
        public async Task<CommonResponse> GetDesignation(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetDesignation(id);
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetUserDetailById")]
        [HasPermission("EditUser")]
        public async Task<CommonResponse> GetUserDetailById(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetUserDetailById(id);
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpPost("GetUserList")]
        [HasPermission("ViewUser")]
        public async Task<CommonResponse> GetUserList(GetUserListRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetUserList(request.Adapt<GetUserListRequestDTO>());
                GetUserListResponseDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<GetUserListResponseModel>();
            }
            catch (Exception ex) { }
            return response;
        }

        [HttpDelete("DeleteUser")]
        [HasPermission("DeleteUser")]
        public async Task<CommonResponse> DeleteUser(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.DeleteUser(id);
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetUserDropDown")]
        public async Task<CommonResponse> GetUserDropDown()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iUser.GetUserDropDown();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }
    }
}
