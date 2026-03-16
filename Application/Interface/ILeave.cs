using Application.DTO.RequestDTO.Leave;
using Helper;

namespace Application.Interface
{
    public interface ILeave
    {
        #region Add Leave Application

        public Task<CommonResponse> GetLeaveStatusList();
        public  Task<CommonResponse> AddEditLeaveApplication(AddEditLeaveApplicationRequestDTO request);
        public Task<CommonResponse> UpdateLeaveApplicationStatus(UpdateLeaveApplicationStatusRequestDTO request);

        #endregion

        #region Leave Type
        public Task<CommonResponse> AddEditLeaveType(AddEditLeaveTypeReqDTO request);
        public Task<CommonResponse> GetLeaveTypeList(GetLeaveTypeListReqDTO request);

        public Task<CommonResponse> DeleteLeaveType(int Id);

        #endregion

        #region Leave Policy

        public Task<CommonResponse> GetLeaveTypeList();


        public Task<CommonResponse> GetLeavePolicyList(GetLeaveTypeListReqDTO request);

        public Task<CommonResponse> AddEditLeavePolicy(AddEditLeavePolicyReqDTO request);



        #endregion
    }
}
