using Application.DTO.RequestDTO.Permission;
using Application.DTO.RequestDTO.User;
using Application.DTO.ResponseDTO.Permission;
using Application.Interface;
using Helper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net;

namespace Application.Services
{
    public class PermissionService : IPermission
    {
        private readonly CommonRepositry _commonRepositry;
        private readonly IConfiguration _configuration;
        private readonly HrmdbContext _context;
        private readonly CommonHelper _commonHelper;
        public PermissionService(CommonRepositry commonRepositry, IConfiguration configuration, HrmdbContext context, CommonHelper commonHelper)
        {
            _commonRepositry = commonRepositry;
            _configuration = configuration;
            _context = context;
            _commonHelper = commonHelper;
        }

        public async Task<CommonResponse> GetAllPermission(GetAllPermissionRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            GetAllPermissionResponseDTO res = new GetAllPermissionResponseDTO();
            try
            {
                int pageIndex = request.PageIndex <= 0 ? Convert.ToInt32(_configuration["Pagination:PageIndex"]) : request.PageIndex;
                int pageSize = request.PageSize <= 0 ? Convert.ToInt32(_configuration["Pagination:PageSize"]) : request.PageIndex;
                bool orderBy = Convert.ToBoolean(_configuration["Pagination:OrderBy"]);

                var roleList = await _commonRepositry.RoleList().Where(x => !x.IsDelete).Select(x => new { x.Id, x.RoleName }).ToListAsync();
                var permissionList = await _commonRepositry.PermissionList().Where(x => !x.IsDelete).Select(x => new { x.Id, x.PermissionName }).ToListAsync();
                var rolePermissionList = await _commonRepositry.RolePermissionList().Where(x => !x.IsDelete).ToListAsync();

                var roleHasPermissionList = new GetAllPermissionResponseDTO
                {
                    TotalCount = rolePermissionList.Count,
                    Roles = roleList.Select(r => new GetAllPermissionResponseDTO.Role
                    {
                        RoleId = r.Id,
                        RoleName = r.RoleName
                    }).ToList(),

                    Permissions = permissionList.Select(p => new GetAllPermissionResponseDTO.PermissionDetail
                    {
                        PermissionId = p.Id,
                        PermissionName = p.PermissionName,

                        RolePermissions = roleList.ToDictionary(
                            role => role.Id,
                            role => rolePermissionList.Any(rp =>
                                rp.RoleId == role.Id &&
                                rp.PermissionId == p.Id
                            )
                        )
                    }).ToList()
                };

                response.Data = roleHasPermissionList;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = "Data found successfully!";

            }
            catch { throw; }
            return response;
        }

        public async Task<CommonResponse> EditPermission(EditPermissionRequestDTO request)
        {
            CommonResponse response = new CommonResponse();
            var userId = _commonHelper.GetUserId();
            try
            {
                var permissionDetail = await _commonRepositry.PermissionList().Where(x => x.Id == request.PermissionId).Select(x => x.Id).FirstOrDefaultAsync();
                if (permissionDetail != 0)
                {
                    var rolePermissionList = await _commonRepositry.RolePermissionList().Where(x => x.PermissionId == request.PermissionId).ToListAsync();

                    _context.RolePermissionMsts.RemoveRange(rolePermissionList);
                    await _context.SaveChangesAsync();

                    RolePermissionMst rolePermissionMst;
                    List<RolePermissionMst> rolePermissionListAdd = new List<RolePermissionMst>();
                    foreach (var item in request.RoleId)
                    {
                        rolePermissionMst = new RolePermissionMst();
                        rolePermissionMst.RoleId = item;
                        rolePermissionMst.PermissionId = request.PermissionId;
                        rolePermissionMst.IsActive = true;
                        rolePermissionMst.IsDelete = false;
                        rolePermissionMst.CreatedAt = DateTime.Now;
                        rolePermissionMst.CreatedBy = userId;
                        rolePermissionMst.UpdatedAt = DateTime.Now;
                        rolePermissionMst.UpdatedBy = userId;

                        rolePermissionListAdd.Add(rolePermissionMst);
                    }

                    await _context.RolePermissionMsts.AddRangeAsync(rolePermissionListAdd);
                    await _context.SaveChangesAsync();

                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Permission updated successfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "PermissionId not found!";
                }
            }
            catch { throw; }
            return response;
        }

    }
}
