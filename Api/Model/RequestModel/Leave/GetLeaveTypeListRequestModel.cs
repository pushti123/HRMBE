namespace Api.Model.RequestModel.Leave
{
    public class GetLeaveTypeListRequestModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool OrderBy { get; set; }

        public string? SearchString { get; set; }

        public string LeaveTypeStatusFilter { get; set; }
    }
}
