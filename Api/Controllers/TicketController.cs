using Api.Model;
using Api.Model.RequestModel;
using Api.Permission;
using Application.DTO.RequestDTO;
using Application.Interface;
using Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _iTicket;
        public TicketController(ITicket ticket)
        {
            _iTicket = ticket;
        }

        [HttpDelete("DeleteTicket")]
        [HasPermission("DeleteTicket")]
        public async Task<CommonResponse> DeleteTicket(int TicketId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iTicket.DeleteTicket(TicketId);
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetTicketList")]
        [HasPermission("ViewTicket")]
        public async Task<CommonResponse> GetTicketList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iTicket.GetTicketList();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetTicketById")]
        [HasPermission("EditTicket")]
        public async Task<CommonResponse> GetTicketById(int TicketId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iTicket.GetTicketById(TicketId);
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpPost("AddEditTicket")]
        [HasPermission("AddTicket", "EditTicket")]
        public async Task<CommonResponse> AddEditTicket(AddTicketReqModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iTicket.AddEditTicket(request.Adapt<AddTicketReqDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;

        }

        [HttpPut("UpdateTicketStatus")]
        [HasPermission("UpdateTicketStatus")]
        public async Task<CommonResponse> UpdateTicketStatus(UpdateTicketStatusReqModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iTicket.UpdateTicketStatus(request.Adapt<UpdateTicketStatusReqDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetStatusList")]
        public async Task<CommonResponse> GetStatusList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iTicket.GetStatusList();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }
    }
}
