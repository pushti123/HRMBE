using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.User;
using Application.Interface;
using Helper;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Application.Services
{
    public class UserService : IUser
    {
        private readonly CommonRepositry _commonRepositry;
        private readonly IConfiguration _configuration;
        private readonly HrmdbContext _context;
        private readonly CommonHelper _commonHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int userId;
        public UserService(CommonRepositry commonRepositry, IConfiguration configuration, HrmdbContext context, CommonHelper commonHelper,IHttpContextAccessor httpContextAccessor)
        {
            _commonRepositry = commonRepositry;
            _configuration = configuration;
            _context = context;
            _commonHelper = commonHelper;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<CommonResponse> AddEditUserPersonalInfo(AddEditUserPersonalInfoRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                userId = _commonHelper.GetUserId();
                if (request.Id > 0)
                {
                    var userDetail = await _commonRepositry.UserList().FirstOrDefaultAsync(x => x.Id == request.Id);
                    if (userDetail != null)
                    {
                        userDetail.Email = request.Email;
                        userDetail.Password = request.Password;
                        userDetail.ConfirmPassword = request.ConfirmPassword;
                        userDetail.AadharNumber = request.AadharNumber;
                        userDetail.Gender = request.Gender;
                        userDetail.UserId = request.UserId;
                        userDetail.Contact = request.ContactNo;
                        userDetail.UpdatedAt = DateTime.Now;
                        userDetail.UpdatedBy = userId;
                        userDetail.Fullname = request.FullName;
                        userDetail.Address = request.Address;
                        userDetail.Pincode = request.PinCode;

                        if (request.IsChanged == true)
                        {
                            if (request.ProfilePic != null && request.ProfilePic.Length > 0)
                            {
                                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                                string folderPath = Path.Combine(wwwRootPath, "UserAttachment");

                                if (!string.IsNullOrEmpty(userDetail.ProfilePic))
                                {
                                    string oldFilePath = Path.Combine(wwwRootPath, userDetail.ProfilePic.TrimStart('/'));

                                    if (System.IO.File.Exists(oldFilePath))
                                        System.IO.File.Delete(oldFilePath);   // remove old file
                                }
                                // Generate unique file name
                                string fileName = userDetail.UserId + Path.GetExtension(request.ProfilePic.FileName);

                                // Full path for save
                                string filePath = Path.Combine(folderPath, fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await request.ProfilePic.CopyToAsync(stream);
                                }
                                userDetail.ProfilePic = $"/UserAttachment/{fileName}";
                            }
                        }

                        _context.Entry(userDetail).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        response.Data = userDetail.Id;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Status = true;
                        response.Message = "User updated successfully!";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Status = false;
                        response.Message = "UserId not found!";
                    }

                }
                else
                {
                    UserMst userMst = new UserMst();
                    userMst.Email = request.Email;
                    userMst.Password = request.Password;
                    userMst.ConfirmPassword = request.ConfirmPassword;
                    userMst.AadharNumber = request.AadharNumber;
                    userMst.Gender = request.Gender;
                    userMst.UserId = request.UserId;
                    userMst.Contact = request.ContactNo;
                    userMst.IsActive = true;
                    userMst.CreatedAt = DateTime.Now;
                    userMst.CreatedBy = userId;
                    userMst.IsDelete = false;
                    userMst.UpdatedAt = DateTime.Now;
                    userMst.UpdatedBy = userId;
                    userMst.Pincode = request.PinCode;
                    userMst.Fullname = request.FullName;
                    userMst.Address = request.Address;

                    if (request.ProfilePic != null && request.ProfilePic.Length > 0)
                    {
                        string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string folderPath = Path.Combine(wwwRootPath, "UserAttachment");

                        string fileName = userMst.UserId + Path.GetExtension(request.ProfilePic.FileName);

                        // Full path for save
                        string filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.ProfilePic.CopyToAsync(stream);
                        }
                        userMst.ProfilePic = $"/UserAttachment/{fileName}";
                    }

                    await _context.UserMsts.AddAsync(userMst);
                    await _context.SaveChangesAsync();

                    response.Data = userMst.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "User added Successfully!";
                }
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> EditUserEmployeeInfo(EditUserEmployeeInfoRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                userId = _commonHelper.GetUserId();

                var userDetail = await _commonRepositry.UserList().FirstOrDefaultAsync(x => x.Id == request.Id);
                if (userDetail != null)
                {
                    userDetail.UpdatedAt = DateTime.Now;
                    userDetail.UpdatedBy = userId;
                    userDetail.DepartmentId = request.DepartmentId;
                    userDetail.RoleId = request.RoleId;
                    userDetail.Designation = request.DesignationId;
                    _context.Entry(userDetail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    response.Data = userDetail.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "User updated successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "UserId not found!";
                }

            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> EditUserBankInfo(EditUserBankInfoRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                userId = _commonHelper.GetUserId();

                var userDetail = await _commonRepositry.UserList().FirstOrDefaultAsync(x => x.Id == request.Id);
                if (userDetail != null)
                {
                    userDetail.UpdatedAt = DateTime.Now;
                    userDetail.UpdatedBy = userId;
                    userDetail.BankName = request.BankName;
                    userDetail.BankNumber = request.BankNumber;
                    _context.Entry(userDetail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    response.Data = userDetail.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "User updated successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "UserId not found!";
                }

            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> GetUserId()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userdetail = await _commonRepositry.UserList().OrderBy(x => x.Id).Select(x => x.Id).LastOrDefaultAsync();
                var userId = $"0000{userdetail + 1}";
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = "UserId found successfully!";
                response.Data = userId;
            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> GetRole()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var roleList = await _commonRepositry.RoleList().Where(x => !x.IsDelete).Select(x => new { x.Id, x.RoleName }).ToListAsync();
                if (roleList.Count > 0)
                {
                    response.Data = roleList;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Role Found Sucessfully!";
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

        public async Task<CommonResponse> GetDepartment()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var departmentList = await _commonRepositry.DepartmentList().Where(x => !x.IsDelete).Select(x => new { x.Id, x.DepartmentName }).ToListAsync();
                if (departmentList.Count > 0)
                {
                    response.Data = departmentList;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Department Found Sucessfully!";
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

        public async Task<CommonResponse> GetDesignation(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                bool departmentExists = await _commonRepositry.DepartmentList().AnyAsync(X => !X.IsDelete && X.Id == id);
                if (departmentExists)
                {
                    var designationList = await _commonRepositry.DesignationList().Where(x => !x.IsDelete && x.DepartmentId == id).Select(x => new { x.Id, x.DesignationName }).ToListAsync();
                    if (designationList.Count > 0)
                    {
                        response.Data = designationList;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Status = true;
                        response.Message = "Designation Found Sucessfully!";
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

        public async Task<CommonResponse> GetUserDetailById(int id)
         {
            CommonResponse response = new CommonResponse();
            try
            {

                var userDetail = await _commonRepositry.UserList().Where(x => x.Id == id).
                    Select(x => new
                    {
                        UserId = x.UserId,
                        FullName = x.Fullname,
                        BankName = x.BankName,
                        BankNumber = x.BankNumber,
                        Id = x.Id,
                        Address = x.Address,
                        PinCode = x.Pincode,
                        AadharNumber = x.AadharNumber,
                        RoleId = x.RoleId,
                        DepartmentId = x.DepartmentId,
                        DesignationId = x.Designation,
                        Email = x.Email,
                        Password = x.Password,
                        ContactNo = x.Contact,
                        Gender = x.Gender,
                        ProfilePic = string.IsNullOrEmpty(x.ProfilePic) ? null: x.ProfilePic,

                    }).FirstOrDefaultAsync();
                if (userDetail != null)
                {
                    response.Data = userDetail;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "UserDetail Found Sucessfully!";
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

        public async Task<CommonResponse> GetUserDropDown()
         {
            CommonResponse response = new CommonResponse();
            try
            {

                var userDetail = await _commonRepositry.UserList().
                    Select(x => new
                    {

                        userName = x.Fullname,
                        id = x.Id,

                    }).ToListAsync();
                if (userDetail.Count > 0)
                {
                    response.Data = userDetail;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "UserDetail Found Sucessfully!";
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

        public async Task<CommonResponse> GetUserList(GetUserListRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            GetUserListResponseDTO responseDTO = new GetUserListResponseDTO();
            try
            {
                int pageIndex = request.PageIndex <= 0 ? Convert.ToInt32(_configuration["Pagination:PageIndex"]) : request.PageIndex;
                int pageSize = request.PageSize <= 0 ? Convert.ToInt32(_configuration["Pagination:PageSize"]) : request.PageIndex;
                bool orderBy = Convert.ToBoolean(_configuration["Pagination:OrderBy"]);

                var userList =  await (from user in _commonRepositry.UserList().Where(x => !x.IsDelete)
                                      join role in _commonRepositry.RoleList().Where(x => !x.IsDelete) on user.RoleId equals role.Id into roleGroup
                                      from role in roleGroup.DefaultIfEmpty()
                                      join department in _commonRepositry.DepartmentList().Where(x => !x.IsDelete) on user.DepartmentId equals department.Id into departmentGroup
                                      from department in departmentGroup.DefaultIfEmpty()
                                      join designation in _commonRepositry.DesignationList().Where(x => !x.IsDelete) on user.Designation equals designation.Id into designationGroup
                                      from designation in designationGroup.DefaultIfEmpty()

                                      select new GetUserListResponseDTO.UserDetail
                                      {
                                          UserId = user.UserId,
                                          RoleName = user.RoleId != null ? role.RoleName  : "-",
                                          DepartmentName = user.DepartmentId != null ? department.DepartmentName  : "-",
                                          DesignationName = user.Designation != null ? designation.DesignationName : "-" ,
                                          Email = user.Email,
                                          FullName = user.Fullname,
                                          Id = user.Id,
                                      }
                    ).ToListAsync();

                userList = orderBy ? userList.OrderBy(x => x.Id).ToList() : userList.OrderByDescending(x => x.Id).ToList();
                responseDTO.TotalCount = userList.Count;
                //userList = userList.Skip(pageIndex * (pageSize - 1)).Take(pageIndex).ToList();

                if (responseDTO.TotalCount > 0)
                {
                    responseDTO.UserDetails = userList;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Data found Successfully!";
                    response.Status = true;
                    response.Data = responseDTO;
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

        public async Task<CommonResponse> DeleteUser(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                userId = _commonHelper.GetUserId();

                var userDetail = await _commonRepositry.UserList().FirstOrDefaultAsync(x => x.Id == id);
                if (userDetail != null)
                {
                   userDetail.UpdatedAt = DateTime.Now;
                    userDetail.UpdatedBy = userId;
                    userDetail.IsDelete = true;
                    _context.Entry(userDetail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    response.Data = userDetail.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "User deleted successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "UserId not found!";
                }

            }
            catch { throw; }
            return response;
        }

    }
}
