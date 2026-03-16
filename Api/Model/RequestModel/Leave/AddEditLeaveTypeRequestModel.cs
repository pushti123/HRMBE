namespace Api.Model.RequestModel.Leave
{
    public class AddEditLeaveTypeRequestModel
    {
        public int Id { get; set; }
        public string LeaveName { get; set; } = null!;

        public string LeaveCode { get; set; } = null!;

        public string Description { get; set; } = null!;
        public bool IsAutoAssign { get; set; }
    }
}
