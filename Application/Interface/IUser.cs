using Application.DTO.RequestDTO.User;
using Helper;

namespace Application.Interface
{
    public interface IUser
    {
        public Task<CommonResponse> AddEditUserPersonalInfo(AddEditUserPersonalInfoRequestDTO request);
        public Task<CommonResponse> EditUserEmployeeInfo(EditUserEmployeeInfoRequestDTO request);
        public Task<CommonResponse> EditUserBankInfo(EditUserBankInfoRequestDTO request);
        public Task<CommonResponse> GetUserId();
        public Task<CommonResponse> GetRole();
        public Task<CommonResponse> GetDepartment();
        public Task<CommonResponse> GetDesignation(int id);
        public Task<CommonResponse> GetUserDetailById(int id);
        public Task<CommonResponse> GetUserList(GetUserListRequestDTO request);
        public Task<CommonResponse> DeleteUser(int id);
        public Task<CommonResponse> GetUserDropDown();
    }
}
