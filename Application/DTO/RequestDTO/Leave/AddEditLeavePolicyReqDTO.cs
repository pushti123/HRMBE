namespace Application.DTO.RequestDTO.Leave
{
    public class AddEditLeavePolicyReqDTO
    {
        public int LeavePolicyId { get; set; }

        public int LeaveTypeId { get; set; }

        public bool? IsProbationApplicable { get; set; }

        public int? ProbationLeaveDays { get; set; }

        public string? LeaveCreditType { get; set; }

        public int? LeaveDays { get; set; }

        public bool? IsCarryForward { get; set; }

        public int? MaxCarryForwardDays { get; set; }

        public bool? IsSandwichApplicable { get; set; }

        public bool? IsHolidaySandwichApplicable { get; set; }

        public bool? IsWeeKOffSandwichApplicable { get; set; }

        public int? MaxLeavePerRequest { get; set; }

        public bool? IsHalfDayAllowed { get; set; }
    }
}
