namespace Api.Model.RequestModel.Leave
{
    public class AddEditLeaveApplicationRequestModel
    {
        public int LeaveApplicationId { get; set; }

        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public decimal TotalDays { get; set; }


        public string? Reason { get; set; }

        public List<LeaveDates> LeaveDateDetails { get; set; }

        public class LeaveDates
        {
            public DateOnly LeaveDate { get; set; }

            public string LeaveDayType { get; set; }

        }
    }
}
