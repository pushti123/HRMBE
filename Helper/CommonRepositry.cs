using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public class CommonRepositry
    {
        private readonly HrmdbContext _HRMDBContext;
        public CommonRepositry(HrmdbContext hRMDBContext)
        {
            _HRMDBContext = hRMDBContext;
        }

        public IQueryable<PermissionMst> PermissionList()
        {
            return _HRMDBContext.PermissionMsts.AsQueryable();
        }
        public IQueryable<RolePermissionMst> RolePermissionList()
        {
            return _HRMDBContext.RolePermissionMsts.AsQueryable();
        }
        public IQueryable<TokenMst> TokenList()
        {
            return _HRMDBContext.TokenMsts.AsQueryable();
        }
        public IQueryable<UserMst> UserList()
        {
            return _HRMDBContext.UserMsts.AsQueryable();
        }
        public IQueryable<RoleMst> RoleList()
        {
            return _HRMDBContext.RoleMsts.AsQueryable();
        }
        public IQueryable<DepartmentMst> DepartmentList()
        {
            return _HRMDBContext.DepartmentMsts.AsQueryable();
        }
        public IQueryable<DesignationMst> DesignationList()
        {
            return _HRMDBContext.DesignationMsts.AsQueryable();
        }

        public IQueryable<TicketMst>TicketList()
        {
            return _HRMDBContext.TicketMsts.AsQueryable();
        }

        public IQueryable <LeaveType> LeaveTypeMstList()
        {
            return _HRMDBContext.LeaveTypes.AsQueryable();
        }

        public IQueryable<LeavePolicy> LeavePolicyMstList()
        {
            return _HRMDBContext.LeavePolicies.AsQueryable();
        }

        public IQueryable<EmployeeLeaveBalance> EmployeeLeaveBalanceList()
        {
            return _HRMDBContext.EmployeeLeaveBalances.AsQueryable();
        }
        public IQueryable<LeaveApplication> LeaveMstList()
        {
            return _HRMDBContext.LeaveApplications.AsQueryable();
        }

        public IQueryable<Holiday> HolidayList()
        {
            return _HRMDBContext.Holidays.AsQueryable();
        }
        public IQueryable<LeaveApplicationDetail> LeaveApplicationDetailList()
        {
            return _HRMDBContext.LeaveApplicationDetails.AsQueryable();
        }
        public IQueryable<Candidate> CandidateList()
        {
            return _HRMDBContext.Candidates.AsQueryable();
        }
        public IQueryable<CandidateEducation> CandidateEducationList()
        {
            return _HRMDBContext.CandidateEducations.AsQueryable();
        }
        public IQueryable<CandidateSkill> CandidateSkillList()
        {
            return _HRMDBContext.CandidateSkills.AsQueryable();
        }
        public IQueryable<CandidateWorkExperience> CandidateWorkExperienceList()
        {
            return _HRMDBContext.CandidateWorkExperiences.AsQueryable();
        }
        public IQueryable<CandidateLanguage> CandidateLanguageList()
        {
            return _HRMDBContext.CandidateLanguages.AsQueryable();
        }
        public IQueryable<CandidateResume> CandidateResumeList()
        {
            return _HRMDBContext.CandidateResumes.AsQueryable();
        }
        public IQueryable<JobApplication> JobApplicationList()
        {
            return _HRMDBContext.JobApplications.AsQueryable();
        }
        public IQueryable<ApplicationQuestion> ApplicationQuestionList()
        {
            return _HRMDBContext.ApplicationQuestions.AsQueryable();
        }
        public IQueryable<ApplicationDeclaration> ApplicationDeclarationsList()
        {
            return _HRMDBContext.ApplicationDeclarations.AsQueryable();
        }
        public IQueryable<ResumeParserUser> ResumeParserUserList()
        {
            return _HRMDBContext.ResumeParserUsers.AsQueryable();
        }
    }
}
