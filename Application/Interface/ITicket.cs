using Application.DTO.RequestDTO;
using Helper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Application.Interface
{
    public interface ITicket
    {
        public Task<CommonResponse> DeleteTicket(int TicketId);

        public Task<CommonResponse> GetTicketList();


        public Task<CommonResponse> GetTicketById(int TicketId);


        public Task<CommonResponse> AddEditTicket(AddTicketReqDTO request);

        public Task<CommonResponse> UpdateTicketStatus(UpdateTicketStatusReqDTO request);

        public Task<CommonResponse> GetStatusList();

    }
}
