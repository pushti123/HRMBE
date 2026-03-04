using Application.DTO.RequestDTO.Permission;
using Application.DTO.RequestDTO.User;
using Helper;

namespace Application.Interface
{
    public interface IPermission
    {
        public Task<CommonResponse> GetAllPermission(GetAllPermissionRequestDTO request);
        public Task<CommonResponse> EditPermission(EditPermissionRequestDTO request);
    }
}
