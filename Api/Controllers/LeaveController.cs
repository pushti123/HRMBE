using Api.Model.RequestModel.Leave;
using Api.Model.RequestModel.User;
using Api.Model.ResponseModel.Leave;
using Api.Model.ResponseModel.User;
using Api.Permission;
using Application.DTO.RequestDTO.Leave;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.Leave;
using Application.DTO.ResponseDTO.User;
using Application.Interface;
using Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeave _iLeave;
        public LeaveController(ILeave iLeave)
        {
            _iLeave = iLeave;
        }

        [HttpGet("GetLeaveStatusList")]
        [HasPermission("UpdateLeaveStatus")]
        public async Task<CommonResponse> GetLeaveStatusList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.GetLeaveStatusList();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPost("AddEditLeaveApplication")]
        [HasPermission("AddLeave", "EditLeave")]
        public async Task<CommonResponse> AddEditLeaveApplication(AddEditLeaveApplicationRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.AddEditLeaveApplication(request.Adapt<AddEditLeaveApplicationRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPut("UpdateLeaveApplicationStatus")]
        [HasPermission("UpdateLeaveStatus")]
        public async Task<CommonResponse> UpdateLeaveApplicationStatus(UpdateLeaveApplicationStatusRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.UpdateLeaveApplicationStatus(request.Adapt<UpdateLeaveApplicationStatusRequestDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPost("AddEditLeaveType")]
        [HasPermission("AddLeaveType", "EditLeaveType")]
        public async Task<CommonResponse> AddEditLeaveType(AddEditLeaveTypeRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.AddEditLeaveType(request.Adapt<AddEditLeaveTypeReqDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPost("GetAllLeaveTypeList")]
        [HasPermission("ViewLeaveType")]
        public async Task<CommonResponse> GetLeaveTypeList(GetLeaveTypeListRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.GetLeaveTypeList(request.Adapt<GetLeaveTypeListReqDTO>());
                GetLeaveTypeListResDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<GetLeaveTypeListResponseModel>();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpDelete("DeleteLeaveType")]
        [HasPermission("DeleteLeaveType")]
        public async Task<CommonResponse> DeleteLeaveType(int Id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.DeleteLeaveType(Id);
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpGet("GetLeaveTypeList")]
        [HasPermission("ViewLeaveType")]
        public async Task<CommonResponse> GetLeaveTypeList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.GetLeaveTypeList();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPost("AddEditLeavePolicy")]
        [HasPermission("AddLeavePolicy", "EditLeavePolicy")]
        public async Task<CommonResponse> AddEditLeavePolicy(AddEditLeavePolicyRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.AddEditLeavePolicy(request.Adapt<AddEditLeavePolicyReqDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPost("GetLeavePolicyList")]
        [HasPermission("ViewLeavePolicy")]
        public async Task<CommonResponse> GetLeavePolicyList(GetLeaveTypeListRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iLeave.GetLeavePolicyList(request.Adapt<GetLeaveTypeListReqDTO>());
                GetLeavePolicyListResDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<GetLeavePolicyListResponseModel>();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

    }
}
