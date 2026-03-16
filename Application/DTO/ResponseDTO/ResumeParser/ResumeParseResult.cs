namespace Application.DTO.ResponseDTO.ResumeParser
{
    public class ResumeParseResult
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }
        public string? CountryCode {  get; set; }

        public List<string>? Skills { get; set; }

        public List<string>? Languages { get; set; }

        public List<WorkExperienceModel>? WorkExperienceDetail { get; set; }

        public List<EducationModel>? Educations { get; set; }

        public class WorkExperienceModel
        {
            public string? CompanyName { get; set; }

            public string? JobTitle { get; set; }

            public DateOnly? StartDate { get; set; }

            public DateOnly? EndDate { get; set; }

            public bool IsCurrentCompany { get; set; }

            public string? Description { get; set; }
        }

        public class EducationModel
        {
            public string? CollegeName { get; set; }

            public string? Degree { get; set; }

            public string? FieldOfStudy { get; set; }

            public int? StartYear { get; set; }

            public int? EndYear { get; set; }
        }
    }
}
