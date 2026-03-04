using Api.Model.RequestModel.Permission;
using Api.Model.RequestModel.User;
using Api.Model.ResponseModel.Permission;
using Api.Permission;
using Application.DTO.RequestDTO.Permission;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.Permission;
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
    public class PermissionController : ControllerBase
    {
        public readonly IPermission _iPermission;
        public PermissionController(IPermission iPermission)
        {
            _iPermission = iPermission;
        }

        [HttpPost("GetAllPermission")]
        [HasPermission("ViewPermission")]
        public async Task<CommonResponse> GetAllPermission(GetAllPermissionRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iPermission.GetAllPermission(request.Adapt<GetAllPermissionRequestDTO>());
                GetAllPermissionResponseDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<GetAllPermissionResponseModel>();
            }
            catch (Exception ex) { }
            return response;
        }

        [HttpPut("EditPermission")]
        [HasPermission("EditPermission")]
        public async Task<CommonResponse> EditPermission(EditPermissionRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iPermission.EditPermission(request.Adapt<EditPermissionRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


    }
}
