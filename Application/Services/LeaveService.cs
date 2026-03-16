using Application.DTO.RequestDTO.Leave;
using Application.DTO.ResponseDTO.Leave;
using Application.Interface;
using Helper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using static Helper.Enum;
using Enum = System.Enum;


namespace Application.Services
{
    public class LeaveService : ILeave
    {
        public readonly HrmdbContext _hrmdbContext;
        public readonly CommonRepositry _commonRepositry;
        public DateTime currentDateTime;
        private readonly IConfiguration _configuration;
        private readonly CommonHelper _commonHelper;
        private int userId;
        public LeaveService(HrmdbContext hrmdbContext, CommonRepositry commonRepositry, IConfiguration configuration, CommonHelper commonHelper )
        {
            _commonRepositry = commonRepositry;
            _hrmdbContext = hrmdbContext;
            currentDateTime = DateTime.Now;
            _configuration = configuration;
            _commonHelper = commonHelper;
        }

        #region Add Leave Application

        public async Task<CommonResponse> GetLeaveStatusList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var ticketStatusList = Enum.GetValues(typeof(LeaveStatusType)).Cast<LeaveStatusType>()
                                .Select(x => new
                                {
                                    Id = (int)x,
                                    LeaveStatus = x.ToString()
                                })
                                               .ToList();
                if (ticketStatusList.Count > 0)
                {
                    response.Status = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = "Leave ststus found successfully!";
                    response.Data = ticketStatusList;
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.Message = "Data not found!";
                }
            }
            catch { throw; }
            return response;

        }

        public async Task<CommonResponse> AddEditLeaveApplication(AddEditLeaveApplicationRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            int isVerifyPoliiesCount = 0;
            int holidaySandwichDays = 0;
            int weekendSandwichDays = 0;
            decimal paidDays = 0;
            decimal lwpDays = 0;
            bool isAlreadAppliedLeave = false;
            bool isSuccess = false;

            try
            {
                if (request.LeaveApplicationId > 0)
                {
                    using var transaction = await _hrmdbContext.Database.BeginTransactionAsync();

                    var iseXistsApplicationLeave = await _hrmdbContext.LeaveApplications.FindAsync(request.LeaveApplicationId);
                    if (iseXistsApplicationLeave != null)
                    {
                        var checkLeaveBalance = await _commonRepositry.EmployeeLeaveBalanceList().LastOrDefaultAsync(x => x.EmployeeId == request.EmployeeId && x.LeaveTypeId == request.LeaveTypeId);
                        if (checkLeaveBalance != null && checkLeaveBalance.RemainingDays != 0)
                        {
                            var existingLeaves = await (
                                from d in _commonRepositry.LeaveApplicationDetailList().Where(x => x.LeaveApplicationId != iseXistsApplicationLeave.LeaveApplicationId)
                                join a in _commonRepositry.LeaveMstList()
                                    on d.LeaveApplicationId equals a.LeaveApplicationId
                                where a.EmployeeId == request.EmployeeId
                                    && a.Status != "Rejected"
                                    && d.LeaveDate >= request.FromDate.AddDays(-3)
                                    && d.LeaveDate <= request.ToDate.AddDays(3)
                                select new
                                {
                                    d.LeaveDate,
                                    d.LeaveDayType
                                }
                            ).ToListAsync();

                            foreach (var item in request.LeaveDateDetails)
                            {
                                if (existingLeaves.Any(x => x.LeaveDate == item.LeaveDate))
                                {
                                    isAlreadAppliedLeave = true;
                                }
                            }

                            var isLeavePolicyDetails = await _commonRepositry.LeavePolicyMstList().FirstOrDefaultAsync(x => x.LeaveTypeId == request.LeaveTypeId);
                            if (isLeavePolicyDetails != null)
                            {
                                if (isLeavePolicyDetails.MaxLeavePerRequest > 0)
                                {
                                    if (isLeavePolicyDetails.MaxLeavePerRequest < request.TotalDays)
                                    {
                                        response.Status = false;
                                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                        response.Message = $"You can apply{isLeavePolicyDetails.MaxLeavePerRequest} days at time!";
                                        isVerifyPoliiesCount += 1;
                                    }
                                }

                                if (!Convert.ToBoolean(isLeavePolicyDetails.IsHalfDayAllowed))
                                {
                                    bool hasHalfDay = request.LeaveDateDetails.Any(x => x.LeaveDayType == "FirstHalf" || x.LeaveDayType == "SecondHalf");

                                    if (hasHalfDay)
                                    {
                                        response.Status = false;
                                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                        response.Message = $"You can not apply half day leave in this leave this leave type!";
                                        isVerifyPoliiesCount += 1;
                                    }
                                }

                                var leaveDatesDetails = request.LeaveDateDetails.ToDictionary(x => x.LeaveDate, x => x.LeaveDayType);

                                if (Convert.ToBoolean(isLeavePolicyDetails.IsHolidaySandwichApplicable))
                                {
                                    var HolidayList = await _commonRepositry.HolidayList().Where(x => x.HolidayDate >= request.FromDate && x.HolidayDate <= request.ToDate).Select(x => x.HolidayDate).ToListAsync();
                                    if (HolidayList.Count > 0)
                                    {
                                        foreach (var holiday in HolidayList)
                                        {
                                            var prevDate = holiday.AddDays(-1);
                                            var nextDate = holiday.AddDays(1);

                                            if (leaveDatesDetails.ContainsKey(prevDate) && leaveDatesDetails.ContainsKey(nextDate))
                                            {
                                                var prevType = leaveDatesDetails[prevDate];
                                                var nextType = leaveDatesDetails[nextDate];

                                                if (prevType == "FullDay" && nextType == "FullDay")
                                                {
                                                    holidaySandwichDays++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (Convert.ToBoolean(isLeavePolicyDetails.IsWeelOffSandwichApplicable))
                                {

                                    for (var date = request.FromDate; date <= request.ToDate; date = date.AddDays(1))
                                    {
                                        bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday ||
                                                         date.DayOfWeek == DayOfWeek.Sunday;

                                        if (!isWeekend)
                                            continue;

                                        var prevDate = date.AddDays(-1);
                                        var nextDate = date.AddDays(1);

                                        if (leaveDatesDetails.ContainsKey(prevDate) && leaveDatesDetails.ContainsKey(nextDate))
                                        {
                                            var prevType = leaveDatesDetails[prevDate];
                                            var nextType = leaveDatesDetails[nextDate];

                                            if (prevType == "FullDay" && nextType == "FullDay")
                                            {
                                                weekendSandwichDays++;
                                            }
                                        }
                                    }
                                }

                                if (isVerifyPoliiesCount > 0)
                                {
                                    return response;
                                }

                                var totalSandwichDays = holidaySandwichDays + weekendSandwichDays;

                                request.TotalDays += totalSandwichDays;

                                //assigning paid and unpaid days
                                if (checkLeaveBalance.RemainingDays >= request.TotalDays)
                                {
                                    paidDays = request.TotalDays;
                                }
                                else
                                {
                                    paidDays = checkLeaveBalance.RemainingDays;
                                    lwpDays = request.TotalDays - checkLeaveBalance.RemainingDays;
                                }

                                if (isAlreadAppliedLeave)
                                {
                                    response.Status = false;
                                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                    response.Message = "Leave already applied any of this days!";
                                    return response;
                                }


                                iseXistsApplicationLeave.TotalDays = request.TotalDays;
                                iseXistsApplicationLeave.FromDate = request.FromDate;
                                iseXistsApplicationLeave.ToDate = request.ToDate;
                                iseXistsApplicationLeave.LwpDays = lwpDays;
                                iseXistsApplicationLeave.PaidLeaveDays = paidDays;
                                iseXistsApplicationLeave.EmployeeId = request.EmployeeId;
                                iseXistsApplicationLeave.Reason = request.Reason;
                                iseXistsApplicationLeave.LeaveTypeId = request.LeaveTypeId;

                                _hrmdbContext.LeaveApplications.Entry(iseXistsApplicationLeave).State = EntityState.Modified;
                                await _hrmdbContext.SaveChangesAsync();

                                var LeaveApplicationDetailListDelete = await _commonRepositry.LeaveApplicationDetailList().Where(x => x.LeaveApplicationId == iseXistsApplicationLeave.LeaveApplicationId).ToListAsync();
                                if (LeaveApplicationDetailListDelete.Count > 0)
                                {
                                    _hrmdbContext.LeaveApplicationDetails.RemoveRange(LeaveApplicationDetailListDelete);
                                    await _hrmdbContext.SaveChangesAsync();
                                }

                                List<LeaveApplicationDetail> leaveApplicationDetailsList = new List<LeaveApplicationDetail>();
                                foreach (var item in request.LeaveDateDetails)
                                {
                                    LeaveApplicationDetail leaveApplicationDetail = new LeaveApplicationDetail();
                                    leaveApplicationDetail.LeaveDate = item.LeaveDate;
                                    leaveApplicationDetail.LeaveDayType = item.LeaveDayType;
                                    leaveApplicationDetail.LeaveApplicationId = iseXistsApplicationLeave.LeaveApplicationId;
                                    leaveApplicationDetail.CreatedDate = currentDateTime;
                                    leaveApplicationDetailsList.Add(leaveApplicationDetail);
                                }

                                await _hrmdbContext.LeaveApplicationDetails.AddRangeAsync(leaveApplicationDetailsList);
                                await _hrmdbContext.SaveChangesAsync();
                                isSuccess = true;
                                if (isSuccess)
                                {
                                    await transaction.CommitAsync();
                                    response.Status = true;
                                    response.StatusCode = System.Net.HttpStatusCode.OK;
                                    response.Message = "Leave updated successfully!";
                                }
                                else
                                {
                                    await transaction.RollbackAsync();
                                    response.Status = false;
                                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                    response.Message = "Leave not updated!";
                                }

                            }
                            else
                            {
                                response.Status = false;
                                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                response.Message = "Leave Policy is not available!";
                            }

                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            response.Message = "Leave balance in not enough!";
                        }
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Data not found!";
                    }
                }
                else
                {
                    using var transaction = await _hrmdbContext.Database.BeginTransactionAsync();

                    var checkLeaveBalance = await _commonRepositry.EmployeeLeaveBalanceList().LastOrDefaultAsync(x => x.EmployeeId == request.EmployeeId && x.LeaveTypeId == request.LeaveTypeId);
                    if (checkLeaveBalance != null && checkLeaveBalance.RemainingDays != 0)
                    {
                        var existingLeaves = await (
                            from d in _commonRepositry.LeaveApplicationDetailList()
                            join a in _commonRepositry.LeaveMstList()
                                on d.LeaveApplicationId equals a.LeaveApplicationId
                            where a.EmployeeId == request.EmployeeId
                                && a.Status != "Rejected"
                                && d.LeaveDate >= request.FromDate.AddDays(-3)
                                && d.LeaveDate <= request.ToDate.AddDays(3)
                            select new
                            {
                                d.LeaveDate,
                                d.LeaveDayType
                            }
                        ).ToListAsync();

                        foreach (var item in request.LeaveDateDetails)
                        {
                            if (existingLeaves.Any(x => x.LeaveDate == item.LeaveDate))
                            {
                                isAlreadAppliedLeave = true;
                            }
                        }

                        var isLeavePolicyDetails = await _commonRepositry.LeavePolicyMstList().FirstOrDefaultAsync(x => x.LeaveTypeId == request.LeaveTypeId);
                        if (isLeavePolicyDetails != null)
                        {
                            if (isLeavePolicyDetails.MaxLeavePerRequest > 0)
                            {
                                if (isLeavePolicyDetails.MaxLeavePerRequest < request.TotalDays)
                                {
                                    response.Status = false;
                                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                    response.Message = $"You can apply{isLeavePolicyDetails.MaxLeavePerRequest} days at time!";
                                    isVerifyPoliiesCount += 1;
                                }
                            }

                            if (!Convert.ToBoolean(isLeavePolicyDetails.IsHalfDayAllowed))
                            {
                                bool hasHalfDay = request.LeaveDateDetails.Any(x => x.LeaveDayType == "FirstHalf" || x.LeaveDayType == "SecondHalf");

                                if (hasHalfDay)
                                {
                                    response.Status = false;
                                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                    response.Message = $"You can not apply half day leave in this leave this leave type!";
                                    isVerifyPoliiesCount += 1;
                                }
                            }

                            var leaveDatesDetails = request.LeaveDateDetails.ToDictionary(x => x.LeaveDate, x => x.LeaveDayType);

                            if (Convert.ToBoolean(isLeavePolicyDetails.IsHolidaySandwichApplicable))
                            {
                                var HolidayList = await _commonRepositry.HolidayList().Where(x => x.HolidayDate >= request.FromDate && x.HolidayDate <= request.ToDate).Select(x => x.HolidayDate).ToListAsync();
                                if (HolidayList.Count > 0)
                                {
                                    foreach (var holiday in HolidayList)
                                    {
                                        var prevDate = holiday.AddDays(-1);
                                        var nextDate = holiday.AddDays(1);

                                        if (leaveDatesDetails.ContainsKey(prevDate) && leaveDatesDetails.ContainsKey(nextDate))
                                        {
                                            var prevType = leaveDatesDetails[prevDate];
                                            var nextType = leaveDatesDetails[nextDate];

                                            if (prevType == "FullDay" && nextType == "FullDay")
                                            {
                                                holidaySandwichDays++;
                                            }
                                        }
                                    }
                                }
                            }

                            if (Convert.ToBoolean(isLeavePolicyDetails.IsWeelOffSandwichApplicable))
                            {

                                for (var date = request.FromDate; date <= request.ToDate; date = date.AddDays(1))
                                {
                                    bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday ||
                                                     date.DayOfWeek == DayOfWeek.Sunday;

                                    if (!isWeekend)
                                        continue;

                                    var prevDate = date.AddDays(-1);
                                    var nextDate = date.AddDays(1);

                                    if (leaveDatesDetails.ContainsKey(prevDate) && leaveDatesDetails.ContainsKey(nextDate))
                                    {
                                        var prevType = leaveDatesDetails[prevDate];
                                        var nextType = leaveDatesDetails[nextDate];

                                        if (prevType == "FullDay" && nextType == "FullDay")
                                        {
                                            weekendSandwichDays++;
                                        }
                                    }
                                }
                            }

                            if (isVerifyPoliiesCount > 0)
                            {
                                return response;
                            }

                            var totalSandwichDays = holidaySandwichDays + weekendSandwichDays;

                            request.TotalDays += totalSandwichDays;

                            //assigning paid and unpaid days
                            if (checkLeaveBalance.RemainingDays >= request.TotalDays)
                            {
                                paidDays = request.TotalDays;
                            }
                            else
                            {
                                paidDays = checkLeaveBalance.RemainingDays;
                                lwpDays = request.TotalDays - checkLeaveBalance.RemainingDays;
                            }

                            if (isAlreadAppliedLeave)
                            {
                                response.Status = false;
                                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                response.Message = "Leave already applied any of this days!";
                                return response;
                            }

                            LeaveApplication leaveApplication = new LeaveApplication();

                            leaveApplication.TotalDays = request.TotalDays;
                            leaveApplication.FromDate = request.FromDate;
                            leaveApplication.ToDate = request.ToDate;
                            leaveApplication.LwpDays = lwpDays;
                            leaveApplication.PaidLeaveDays = paidDays;
                            leaveApplication.EmployeeId = request.EmployeeId;
                            leaveApplication.Reason = request.Reason;
                            leaveApplication.AppliedDate = currentDateTime;
                            leaveApplication.LeaveTypeId = request.LeaveTypeId;

                            await _hrmdbContext.LeaveApplications.AddAsync(leaveApplication);
                            await _hrmdbContext.SaveChangesAsync();

                            List<LeaveApplicationDetail> leaveApplicationDetailsList = new List<LeaveApplicationDetail>();
                            foreach (var item in request.LeaveDateDetails)
                            {
                                LeaveApplicationDetail leaveApplicationDetail = new LeaveApplicationDetail();
                                leaveApplicationDetail.LeaveDate = item.LeaveDate;
                                leaveApplicationDetail.LeaveDayType = item.LeaveDayType;
                                leaveApplicationDetail.LeaveApplicationId = leaveApplication.LeaveApplicationId;
                                leaveApplicationDetail.CreatedDate = currentDateTime;
                                leaveApplicationDetailsList.Add(leaveApplicationDetail);
                            }

                            await _hrmdbContext.LeaveApplicationDetails.AddRangeAsync(leaveApplicationDetailsList);
                            await _hrmdbContext.SaveChangesAsync();
                            isSuccess = true;
                            if (isSuccess)
                            {
                                await transaction.CommitAsync();
                                response.Status = true;
                                response.StatusCode = System.Net.HttpStatusCode.OK;
                                response.Message = "Leave add successfully!";
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                response.Status = false;
                                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                response.Message = "Leave not added!";
                            }

                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            response.Message = "Leave Policy is not available!";
                        }

                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        response.Message = "Leave balance in not enough!";
                    }
                }

            }
            catch { throw; }
            return response;

        }

        public async Task<CommonResponse> UpdateLeaveApplicationStatus(UpdateLeaveApplicationStatusRequestDTO request)
        {
            CommonResponse response = new CommonResponse();

            decimal paidDays = 0;
            decimal lwpDays = 0;
            bool isSuccess = false;
            userId = _commonHelper.GetUserId();
            using var transaction = await _hrmdbContext.Database.BeginTransactionAsync();

            try
            {
                var userDetail = await _commonRepositry.UserList().Select(x => new {x.Id,x.RoleId}).FirstOrDefaultAsync(x => x.Id == userId);
                var roleDetail = await _commonRepositry.RoleList().Select(x => new { x.Id, x.RoleName }).FirstOrDefaultAsync(x => x.Id == userDetail.RoleId);
                if (roleDetail.RoleName == "HR")
                {
                    var iseXistsApplicationLeave = await _hrmdbContext.LeaveApplications.FindAsync(request.LeaveApplicationId);
                    if (iseXistsApplicationLeave != null)
                    {
                        var checkLeaveBalance = await _commonRepositry.EmployeeLeaveBalanceList().LastOrDefaultAsync(x => x.EmployeeId == iseXistsApplicationLeave.EmployeeId && x.LeaveTypeId == iseXistsApplicationLeave.LeaveTypeId);
                        if (checkLeaveBalance != null)
                        {
                            if (request.Status == "Approved")
                            {
                                EmployeeLeaveBalance employeeLeaveBalance = new EmployeeLeaveBalance();

                                //assigning paid and unpaid days
                                if (checkLeaveBalance.RemainingDays >= iseXistsApplicationLeave.TotalDays)
                                {
                                    paidDays = iseXistsApplicationLeave.TotalDays;
                                    employeeLeaveBalance.RemainingDays = checkLeaveBalance.RemainingDays - (int)iseXistsApplicationLeave.TotalDays;

                                }
                                else
                                {
                                    paidDays = checkLeaveBalance.RemainingDays;
                                    lwpDays = iseXistsApplicationLeave.TotalDays - checkLeaveBalance.RemainingDays;
                                    if (checkLeaveBalance.RemainingDays > 0)
                                    {
                                        employeeLeaveBalance.RemainingDays = checkLeaveBalance.RemainingDays - (int)paidDays;
                                    }
                                    else
                                    {
                                        employeeLeaveBalance.RemainingDays = 0;
                                    }
                                }
                                iseXistsApplicationLeave.LwpDays = lwpDays;
                                iseXistsApplicationLeave.PaidLeaveDays = paidDays;

                                employeeLeaveBalance.LeaveTypeId = iseXistsApplicationLeave.LeaveTypeId;
                                employeeLeaveBalance.EmployeeId = iseXistsApplicationLeave.EmployeeId;
                                employeeLeaveBalance.AllocatedDays = (int)iseXistsApplicationLeave.TotalDays;
                                employeeLeaveBalance.CreatedDate = currentDateTime;
                                employeeLeaveBalance.UsedDays = (int)iseXistsApplicationLeave.TotalDays;

                                await _hrmdbContext.EmployeeLeaveBalances.AddAsync(employeeLeaveBalance);
                                await _hrmdbContext.SaveChangesAsync();

                            }
                            iseXistsApplicationLeave.Status = request.Status;

                            _hrmdbContext.LeaveApplications.Entry(iseXistsApplicationLeave).State = EntityState.Modified;
                            await _hrmdbContext.SaveChangesAsync();


                            isSuccess = true;
                            if (isSuccess)
                            {
                                await transaction.CommitAsync();
                                response.Status = true;
                                response.StatusCode = System.Net.HttpStatusCode.OK;
                                response.Message = "Leave updated successfully!";
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                response.Status = false;
                                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                response.Message = "Leave not updated!";
                            }
                        }
                        else
                        {
                            response.Status = false;
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            response.Message = "Leave balance in not available!";
                        }
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Data not found!";
                    }
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Only HR can change Status!";
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return response;

        }


        #endregion

        #region Leave Type
        public async Task<CommonResponse> AddEditLeaveType(AddEditLeaveTypeReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.Id > 0)
                {
                    var iseXistsLeaveType = await _hrmdbContext.LeaveTypes.FirstOrDefaultAsync(x => x.LeaveTypeId == request.Id);
                    if (iseXistsLeaveType != null)
                    {
                        iseXistsLeaveType.Description = request.Description;
                        iseXistsLeaveType.LeaveName = request.LeaveName;
                        iseXistsLeaveType.UpdatedDate = currentDateTime;
                        iseXistsLeaveType.IsAutoAssign = request.IsAutoAssign;

                        _hrmdbContext.Entry(iseXistsLeaveType).State = EntityState.Modified;
                        await _hrmdbContext.SaveChangesAsync();

                        response.Status = true;
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = "Leave Type Updated Successfully!";
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Data not found!";
                    }
                }
                else
                {
                    Infrastructure.Entities.LeaveType leaveType = new Infrastructure.Entities.LeaveType();
                    leaveType.Description = request.Description;
                    leaveType.LeaveCode = request.LeaveCode;
                    leaveType.LeaveName = request.LeaveName;
                    leaveType.UpdatedDate = currentDateTime;
                    leaveType.CreatedDate = currentDateTime;
                    leaveType.IsActive = true;
                    leaveType.IsDelete = false;
                    leaveType.IsAutoAssign = request.IsAutoAssign;

                    await _hrmdbContext.LeaveTypes.AddAsync(leaveType);
                    await _hrmdbContext.SaveChangesAsync();

                    response.Status = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = "Leave Type Add Successfully!";

                }

            }
            catch { throw; }
            return response;

        }

        public async Task<CommonResponse> GetLeaveTypeList(GetLeaveTypeListReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            GetLeaveTypeListResDTO resDTO = new GetLeaveTypeListResDTO();
            try
            {
                int pageIndex = request.PageIndex <= 0 ? Convert.ToInt32(_configuration["Pagination:PageIndex"]) : request.PageIndex;
                int pageSize = request.PageSize <= 0 ? Convert.ToInt32(_configuration["Pagination:PageSize"]) : request.PageIndex;
                bool orderBy = Convert.ToBoolean(_configuration["Pagination:OrderBy"]);

                var query = _commonRepositry.LeaveTypeMstList().AsNoTracking();

                if (request.LeaveTypeStatusFilter == "Active")
                {
                    query = query.Where(x => x.IsActive == true && x.IsDelete == false);
                }
                else if (request.LeaveTypeStatusFilter == "DeActive")
                {
                    query = query.Where(x => x.IsDelete == true);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchString))
                {
                    query = query.Where(x =>
                        x.LeaveName.Contains(request.SearchString) ||
                        x.LeaveCode.Contains(request.SearchString));
                }

                var leaveTypeList = await query
                    .Select(x => new GetLeaveTypeListResDTO.LeaveType
                    {
                        LeaveTypeId = x.LeaveTypeId,
                        LeaveCode = x.LeaveCode,
                        LeaveName = x.LeaveName,
                        IsAutoAssign = x.IsAutoAssign ?? false
                    })
                    .ToListAsync();

                resDTO.TotalCount = leaveTypeList.Count;

                if (resDTO.TotalCount > 0)
                {
                    resDTO.LeaveTypeDetail = leaveTypeList;

                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Data found Successfully!";
                    response.Status = true;
                    response.Data = resDTO;
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

        public async Task<CommonResponse> DeleteLeaveType(int Id)
        {
            CommonResponse response = new CommonResponse();
            using var transaction = await _hrmdbContext.Database.BeginTransactionAsync();
            try
            {
                var iseXistsLeaveType = await _hrmdbContext.LeaveTypes.FindAsync(Id);
                if (iseXistsLeaveType != null)
                {
                    iseXistsLeaveType.UpdatedDate = currentDateTime;
                    iseXistsLeaveType.IsAutoAssign = false;
                    iseXistsLeaveType.IsDelete = false;

                    _hrmdbContext.Entry(iseXistsLeaveType).State = EntityState.Modified;
                    await _hrmdbContext.SaveChangesAsync();

                    var isExsistsLeavePolicy = await _hrmdbContext.LeavePolicies.FindAsync(Id);
                    if (isExsistsLeavePolicy != null)
                    {
                        isExsistsLeavePolicy.IsDelete = false;
                        isExsistsLeavePolicy.UpdatedDate = currentDateTime;

                        _hrmdbContext.Entry(isExsistsLeavePolicy).State = EntityState.Modified;
                        await _hrmdbContext.SaveChangesAsync();

                        await transaction.CommitAsync();

                        response.Status = true;
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = "Leave Type deleted Successfully!";
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Data not found!";
                    }
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.Message = "Data not found!";
                }

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return response;
        }
        #endregion

        #region Leave Policy

        public async Task<CommonResponse> GetLeaveTypeList()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var leaveTypeList = await _commonRepositry.LeaveTypeMstList()
                   .Select(x => new
                   {
                       Id = x.LeaveTypeId,
                       LeaveTypeName = x.LeaveName
                   })
                   .ToListAsync();


                if (leaveTypeList.Count > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Data found Successfully!";
                    response.Status = true;
                    response.Data = leaveTypeList;
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

        public async Task<CommonResponse> GetLeavePolicyList(GetLeaveTypeListReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            GetLeavePolicyListResDTO resDTO = new GetLeavePolicyListResDTO();
            try
            {
                int pageIndex = request.PageIndex <= 0 ? Convert.ToInt32(_configuration["Pagination:PageIndex"]) : request.PageIndex;
                int pageSize = request.PageSize <= 0 ? Convert.ToInt32(_configuration["Pagination:PageSize"]) : request.PageIndex;
                bool orderBy = Convert.ToBoolean(_configuration["Pagination:OrderBy"]);

                var query = _commonRepositry.LeavePolicyMstList().AsNoTracking();

                if (request.LeaveTypeStatusFilter == "Active")
                {
                    query = query.Where(x => x.IsActive == true && x.IsDelete == false);
                }
                else if (request.LeaveTypeStatusFilter == "DeActive")
                {
                    query = query.Where(x => x.IsDelete == true);
                }


                var leavePolicyList = await (from leavePolicy in query
                                             join leaveType in _commonRepositry.LeaveTypeMstList() on leavePolicy.LeaveTypeId equals leaveType.LeaveTypeId into leaveTypeGroup
                                             from leaveType in leaveTypeGroup.DefaultIfEmpty()

                                             select new GetLeavePolicyListResDTO.LeavePolicy
                                             {
                                                 LeavePolicyId = leavePolicy.LeavePolicyId,
                                                 LeaveTypeName = leaveType.LeaveName,
                                                 IsProbationApplicable = (bool)leavePolicy.IsProbationApplicable,
                                                 LeaveCreditType = leavePolicy.LeaveCreditType,
                                                 LeaveDays = leavePolicy.LeaveDays != null ? (int)leavePolicy.LeaveDays : 0,
                                                 MaxLeavePerRequest = leavePolicy.MaxLeavePerRequest != null ? (int)leavePolicy.MaxLeavePerRequest : 0,
                                             }
                                             ).AsNoTracking().ToListAsync();



                resDTO.TotalCount = leavePolicyList.Count;

                if (resDTO.TotalCount > 0)
                {
                    if (!string.IsNullOrWhiteSpace(request.SearchString))
                    {
                        leavePolicyList = leavePolicyList.Where(x =>
                            x.LeaveTypeName.Contains(request.SearchString)).ToList();
                    }

                    resDTO.TotalCount = leavePolicyList.Count;

                    if (resDTO.TotalCount > 0)
                    {
                        resDTO.LeavePolicyDetail = leavePolicyList;

                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = "Data found Successfully!";
                        response.Status = true;
                        response.Data = resDTO;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Status = false;
                        response.Message = "Data not found!";
                    }
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


        public async Task<CommonResponse> AddEditLeavePolicy(AddEditLeavePolicyReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.LeavePolicyId > 0)
                {
                    var iseXistsLeavePolicy = await _hrmdbContext.LeavePolicies.FindAsync(request.LeavePolicyId);
                    if (iseXistsLeavePolicy != null)
                    {
                        iseXistsLeavePolicy.LeaveTypeId = request.LeaveTypeId;

                        iseXistsLeavePolicy.IsProbationApplicable = request.IsProbationApplicable;
                        iseXistsLeavePolicy.ProbationLeaveDays = request.ProbationLeaveDays;

                        iseXistsLeavePolicy.LeaveCreditType = request.LeaveCreditType;
                        iseXistsLeavePolicy.LeaveDays = request.LeaveDays;

                        iseXistsLeavePolicy.IsCarryForward = request.IsCarryForward;
                        iseXistsLeavePolicy.MaxCarryForwardDays = request.MaxCarryForwardDays;

                        iseXistsLeavePolicy.MaxLeavePerRequest = request.MaxLeavePerRequest;

                        iseXistsLeavePolicy.IsHolidaySandwichApplicable = request.IsSandwichApplicable;
                        iseXistsLeavePolicy.IsHolidaySandwichApplicable = request.IsHolidaySandwichApplicable;
                        iseXistsLeavePolicy.IsWeelOffSandwichApplicable = request.IsWeeKOffSandwichApplicable;

                        iseXistsLeavePolicy.IsHalfDayAllowed = request.IsHalfDayAllowed;

                        iseXistsLeavePolicy.UpdatedDate = currentDateTime;
                        _hrmdbContext.Entry(iseXistsLeavePolicy).State = EntityState.Modified;
                        await _hrmdbContext.SaveChangesAsync();

                        response.Status = true;
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = "Leave Policy Updated Successfully!";
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                        response.Message = "Data not found!";
                    }
                }
                else
                {
                    LeavePolicy LeavePolicy = new LeavePolicy();
                    LeavePolicy.LeaveTypeId = request.LeaveTypeId;

                    LeavePolicy.IsProbationApplicable = request.IsProbationApplicable;
                    LeavePolicy.ProbationLeaveDays = request.ProbationLeaveDays;

                    LeavePolicy.LeaveCreditType = request.LeaveCreditType;
                    LeavePolicy.LeaveDays = request.LeaveDays;

                    LeavePolicy.IsCarryForward = request.IsCarryForward;
                    LeavePolicy.MaxCarryForwardDays = request.MaxCarryForwardDays;

                    LeavePolicy.MaxLeavePerRequest = request.MaxLeavePerRequest;

                    LeavePolicy.IsHolidaySandwichApplicable = request.IsSandwichApplicable;
                    LeavePolicy.IsHolidaySandwichApplicable = request.IsHolidaySandwichApplicable;
                    LeavePolicy.IsWeelOffSandwichApplicable = request.IsWeeKOffSandwichApplicable;

                    LeavePolicy.IsHalfDayAllowed = request.IsHalfDayAllowed;

                    LeavePolicy.UpdatedDate = currentDateTime;
                    LeavePolicy.CreatedDate = currentDateTime;
                    LeavePolicy.IsActive = true;
                    LeavePolicy.IsDelete = false;

                    await _hrmdbContext.LeavePolicies.AddAsync(LeavePolicy);
                    await _hrmdbContext.SaveChangesAsync();

                    response.Status = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = "Leave Policy Add Successfully!";

                }

            }
            catch { throw; }
            return response;

        }


        #endregion

    }
}
