using Application.DTO.RequestDTO;
using Application.Interface;
using Helper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Services 
{
    public class TicketService :ITicket
    {
        private readonly CommonRepositry _commonRepositry;
        private readonly IConfiguration _configuration;
        private readonly HrmdbContext _context;
        private readonly CommonHelper _commonHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int userId;
        public TicketService(CommonRepositry commonRepositry, IConfiguration configuration, HrmdbContext context, CommonHelper commonHelper, IHttpContextAccessor httpContextAccessor)
        {
            _commonRepositry = commonRepositry;
            _configuration = configuration;
            _context = context;
            _commonHelper = commonHelper;
            _httpContextAccessor = httpContextAccessor;
            userId = _commonHelper.GetUserId();
        }

        public async Task<CommonResponse> DeleteTicket(int TicketId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDetail = await _commonRepositry.TicketList().FirstOrDefaultAsync(x => x.Id == TicketId);
                if (userDetail != null)
                {
                    userDetail.Isdeleted = true;
                    userDetail.UpdatedAt = DateTime.Now;

                    _context.Entry(userDetail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Ticket Deleted successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data is invalid!";
                }
            }
            catch { throw; }
            return response;
        }


        public async Task<CommonResponse> GetTicketList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDetail = await (from user in _commonRepositry.TicketList().Where(x => !x.Isdeleted)
                                        select new
                                        {
                                            Id = user.Id,
                                            subject = user.Subject,
                                            Description = user.Description,
                                            Status = user.Status,
                                            Comment = user.Comment,
                                        }).ToListAsync();

                if (userDetail != null)
                {
                    response.Data = userDetail;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Ticket found successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data is invalid!";
                }
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> GetTicketById(int TicketId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDetail = await (from user in _commonRepositry.TicketList()
                                        select new
                                        {
                                            Id = user.Id,
                                            subject = user.Subject,
                                            Description = user.Description,
                                            Status = user.Status,
                                            Comment = user.Comment,
                                            UserId = user.UserId,
                                        }).FirstOrDefaultAsync(x => x.Id == TicketId);

                if (userDetail != null)
                {
                    response.Data = userDetail;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Ticket found successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data is invalid!";
                }
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> AddEditTicket(AddTicketReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.Id == 0)
                {
                    TicketMst userMst = new TicketMst();
                    userMst.Subject = request.Subject;
                    userMst.Description = request.Description;
                    userMst.CreatedAt = DateTime.Now;
                    userMst.CreatedBy = request.UserId;
                    userMst.UserId = request.UserId;
                    userMst.Isdeleted = false;
                    userMst.UpdatedAt = DateTime.Now;
                    userMst.UpdatedBy = request.UserId;
                    userMst.Status = "ToDo";
                    userMst.Comment = "Added by the backend as a dummy";


                    await _context.TicketMsts.AddAsync(userMst);
                    await _context.SaveChangesAsync();

                    response.Data = userMst.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Ticket addded successfully!";
                }
                else
                {
                    var userDetail = await _commonRepositry.TicketList().FirstOrDefaultAsync(x => x.Id == request.Id);
                    if (userDetail != null)
                    {
                        userDetail.Subject = request.Subject;
                        userDetail.Description = request.Description;
                        userDetail.UpdatedBy = request.UserId;
                        userDetail.UpdatedAt = DateTime.Now;


             
                        _context.Entry(userDetail).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        response.Data = userDetail.Id;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Status = true;
                        response.Message = "Ticket updated successfully!";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Status = false;
                        response.Message = "Data not found!";
                    }
                }
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> UpdateTicketStatus(UpdateTicketStatusReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDetail = await _commonRepositry.TicketList().FirstOrDefaultAsync(x => x.Id == request.Id);
                if (userDetail != null)
                {
                    userDetail.Status = request.Status;
                    userDetail.Comments = request.Comment;
                    userDetail.UpdatedAt = DateTime.Now;

                    _context.Entry(userDetail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    response.Data = userDetail.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Ticket status updated successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data not found!";
                }
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> GetStatusList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var statusList = Enum.GetValues(typeof(Status))
                                             .Cast<Status>()
                                             .Select(s => new { Id = (int)s, StatusName = s.ToString() })
                                             .ToList();
                if (statusList != null)
                {
                    response.Data = statusList;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Status found successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data not found!";
                }
            }
            catch { throw; }
            return response;
        }

        private enum Status
        {
            ToDo = 1,
            InProgress = 2,
            Done = 3
        }


    }
}
