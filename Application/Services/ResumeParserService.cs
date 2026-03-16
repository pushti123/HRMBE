using Application.DTO.RequestDTO.ResumeParser;
using Application.DTO.ResponseDTO.ResumeParser;
using Application.Interface;
using Helper;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;


namespace Application.Services
{
    public class ResumeParserService : IResumeParser
    {
        public readonly HrmdbContext _hrmdbContext;
        public readonly CommonRepositry _commonRepositry;
        public DateTime currentDateTime;
        private readonly IConfiguration _configuration;
        private readonly CommonHelper _commonHelper;
        private int userId;


        public ResumeParserService(HrmdbContext hrmdbContext, CommonRepositry commonRepositry, IConfiguration configuration, CommonHelper commonHelper)
        {
            _commonRepositry = commonRepositry;
            _hrmdbContext = hrmdbContext;
            currentDateTime = DateTime.Now;
            _configuration = configuration;
            _commonHelper = commonHelper;
        }

        public async Task<CommonResponse> AddDEtailByResumeParser(AddDEtailByResumeParserReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            userId = _commonHelper.GetUserId();
            //userId = 1;
            try
            {
                if (request.FileUplaod != null)
                {
                    string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string folderPath = Path.Combine(wwwRootPath, "CandidateResume");


                    // Generate unique file name
                    string fileName = "resumeparser_" + request.JobId + Path.GetExtension(request.FileUplaod.FileName);

                    // Full path for save
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.FileUplaod.CopyToAsync(stream);
                    }
                    //userDetail.ProfilePic = $"/UserAttachment/{fileName}";

                    var resumeText = _commonHelper.ExtractText(filePath);

                    var parsed = await ParseResume(resumeText);
                    if (parsed != null)
                    {

                        Candidate candidate = new Candidate();
                        candidate.Email = parsed.Email;
                        candidate.FirstName = parsed.FirstName;
                        candidate.LastName = parsed.LastName;
                        candidate.Mobile = parsed.Mobile;
                        candidate.CountryCode = parsed.CountryCode;
                        candidate.ResumeParserUserId = userId;
                        candidate.CreatedDate = currentDateTime;
                        candidate.IsActive = true;
                        candidate.FinalSubmitted = false;

                        await _hrmdbContext.Candidates.AddAsync(candidate);
                        await _hrmdbContext.SaveChangesAsync();

                        CandidateResume candidateResume = new CandidateResume();
                        candidateResume.CandidateId = candidate.CandidateId;
                        candidateResume.JobId = request.JobId;
                        candidateResume.FilePath = $"/CandidateResume/{fileName}";
                        candidateResume.CreatedDate = currentDateTime;
                        candidateResume.FileName = fileName;
                        candidateResume.ParsedJson = JsonConvert.SerializeObject(parsed);

                        await _hrmdbContext.CandidateResumes.AddAsync(candidateResume);
                        await _hrmdbContext.SaveChangesAsync();

                        JobApplication jobApplication = new JobApplication();
                        jobApplication.CandidateId = candidate.CandidateId;
                        jobApplication.JobId = request.JobId;
                        jobApplication.ResumeId = candidateResume.ResumeId;
                        jobApplication.CreatedDate = currentDateTime;
                        jobApplication.JobTitle = "dummy";
                        jobApplication.IsActive = true;
                        jobApplication.FinalSubmitted = false;
                        jobApplication.ResumeParserUserId = userId;

                        await _hrmdbContext.JobApplications.AddAsync(jobApplication);
                        await _hrmdbContext.SaveChangesAsync();

                        if (parsed.Skills.Count > 0)
                        {
                            List<CandidateSkill> candidateSkillList = new List<CandidateSkill>();

                            foreach (var item in parsed.Skills)
                            {
                                CandidateSkill candidateSkill = new CandidateSkill();
                                candidateSkill.CandidateId = candidate.CandidateId;
                                candidateSkill.JobApplicationId = jobApplication.JobApplicationId;
                                candidateSkill.SkillName = item;
                                candidateSkillList.Add(candidateSkill);
                            }
                            if (candidateSkillList.Count > 0)
                            {
                                await _hrmdbContext.CandidateSkills.AddRangeAsync(candidateSkillList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }

                        if (parsed.Languages.Count > 0)
                        {
                            List<CandidateLanguage> candidateLanguageList = new List<CandidateLanguage>();

                            foreach (var item in parsed.Languages)
                            {
                                CandidateLanguage candidateLanguage = new CandidateLanguage();
                                candidateLanguage.CandidateId = candidate.CandidateId;
                                candidateLanguage.JobApplicationId = jobApplication.JobApplicationId;
                                candidateLanguage.LanguageName = item;
                                candidateLanguageList.Add(candidateLanguage);
                            }
                            if (candidateLanguageList.Count > 0)
                            {
                                await _hrmdbContext.CandidateLanguages.AddRangeAsync(candidateLanguageList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }

                        if (parsed.WorkExperienceDetail != null)
                        {
                            List<CandidateWorkExperience> candidateWorkExperinceList = new List<CandidateWorkExperience>();

                            foreach (var item in parsed.WorkExperienceDetail)
                            {
                                CandidateWorkExperience candidateWorkExperince = new CandidateWorkExperience();
                                candidateWorkExperince.CandidateId = candidate.CandidateId;
                                candidateWorkExperince.JobApplicationId = jobApplication.JobApplicationId;
                                candidateWorkExperince.JobId = request.JobId;
                                candidateWorkExperince.CandidateWorkExperiencesJobTitle = item.JobTitle;
                                candidateWorkExperince.CompanyName = item.CompanyName;
                                candidateWorkExperince.StartDate = item.StartDate;
                                candidateWorkExperince.EndDate = item.EndDate;
                                candidateWorkExperince.IsCurrentCompany = item.IsCurrentCompany;
                                candidateWorkExperince.Description = item.Description;
                                candidateWorkExperinceList.Add(candidateWorkExperince);
                            }
                            if (candidateWorkExperinceList.Count > 0)
                            {
                                await _hrmdbContext.CandidateWorkExperiences.AddRangeAsync(candidateWorkExperinceList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }

                        if (parsed.Educations.Count > 0 || parsed.Educations != null)
                        {
                            List<CandidateEducation> candidateEducationList = new List<CandidateEducation>();

                            foreach (var item in parsed.Educations)
                            {
                                CandidateEducation candidateEducation = new CandidateEducation();
                                candidateEducation.CandidateId = candidate.CandidateId;
                                candidateEducation.JobApplicationId = jobApplication.JobApplicationId;
                                candidateEducation.CollegeName = item.CollegeName;
                                candidateEducation.Degree = item.Degree;
                                candidateEducation.FieldOfStudy = item.FieldOfStudy;
                                candidateEducation.StartYear = item.StartYear;
                                candidateEducation.EndYear = item.EndYear;
                                candidateEducationList.Add(candidateEducation);
                            }
                            if (candidateEducationList.Count > 0)
                            {
                                await _hrmdbContext.CandidateEducations.AddRangeAsync(candidateEducationList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }

                        response.Data = candidate.CandidateId;
                        response.Status = true;
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = "Resume parsing done!";
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        response.Message = "Something went wrong!";
                    }
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Please upload Resume!";
                }
            }
            catch { throw; }
            return response;

        }

        public async Task<ResumeParseResult> ParseResume(string resumeText)
        {
            //resumeText = resumeText.Length > 4000 ? resumeText.Substring(0, 4000) : resumeText;
            var prompt = $@"
                            You are a resume parser.

                            Extract structured information from the resume and return ONLY VALID JSON.

                            Follow this JSON schema exactly:

                            {{
                              ""FirstName"": ""string"",
                              ""LastName"": ""string"",
                              ""Email"": ""string"",
                              ""Mobile"": ""string"",
                              ""CountryCode"": ""string"",
                              ""Skills"": [""string""],
                              ""Languages"": [""string""],
                              ""WorkExperienceDetail"": [
                                {{
                                  ""CompanyName"": ""string"",
                                  ""JobTitle"": ""string"",
                                  ""StartDate"": ""yyyy-MM-dd"",
                                  ""EndDate"": ""yyyy-MM-dd"",
                                  ""IsCurrentCompany"": true,
                                  ""Description"": ""string""
                                }}
                              ],
                              ""Educations"": [
                                {{
                                  ""CollegeName"": ""string"",
                                  ""Degree"": ""string"",
                                  ""FieldOfStudy"": ""string"",
                                  ""StartYear"": 0,
                                  ""EndYear"": 0
                                }}
                              ]
                            }}

                            Rules:
                            - Convert month-year to date format yyyy-MM-dd
                            - Example: ""January 2023"" → ""2023-01-01""
                            - Example: ""August 2023 - April 2025"" →
                              StartDate: ""2023-08-01"",
                              EndDate: ""2025-04-01""
                            - If currently working, set IsCurrentCompany=true and EndDate=null
                            - Combine responsibilities into Description text
                            - Do NOT return explanation
                            - Return ONLY JSON

                            Resume:
                            {resumeText}
                            ";

            var http = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10)
            };

            var body = new
            {
                model = "llama3",
                prompt = prompt,
                stream = false
            };

            var response = await http.PostAsJsonAsync(
                "http://localhost:11434/api/generate",
                body
            );


            var result = await response.Content.ReadAsStringAsync();


            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

            string text = data.response;

            int start = text.IndexOf("{");
            int end = text.LastIndexOf("}") + 1;

            string json = text.Substring(start, end - start);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<ResumeParseResult>(json);
        }

        public async Task<CommonResponse> GetResumeParserDetailById(int id)
        {
            //userId = _commonHelper.GetUserId();
            userId = 1;
            CommonResponse response = new CommonResponse();
            try
            {
                var resumeParserDetail = await (
                                       from candidate in _commonRepositry.CandidateList().Where(x => x.ResumeParserUserId == id)

                                       join candidateEducations in _commonRepositry.CandidateEducationList()
                                           on candidate.CandidateId equals candidateEducations.CandidateId into candidateEducationGroup
                                       from candidateEducations in candidateEducationGroup.DefaultIfEmpty()

                                       join candidateLanguages in _commonRepositry.CandidateLanguageList()
                                           on candidate.CandidateId equals candidateLanguages.CandidateId into candidateLanguageGroup
                                       from candidateLanguages in candidateLanguageGroup.DefaultIfEmpty()

                                       join candidateWorkExperince in _commonRepositry.CandidateWorkExperienceList()
                                           on candidate.CandidateId equals candidateWorkExperince.CandidateId into candidateWorkExperinceGroup
                                       from candidateWorkExperince in candidateWorkExperinceGroup.DefaultIfEmpty()

                                       join candidateSkill in _commonRepositry.CandidateSkillList()
                                           on candidate.CandidateId equals candidateSkill.CandidateId into candidateSkillGroup
                                       from candidateSkill in candidateSkillGroup.DefaultIfEmpty()

                                       join candidateCandidateResumeList in _commonRepositry.CandidateResumeList()
                                           on candidate.CandidateId equals candidateCandidateResumeList.CandidateId into candidateCandidateResumeListGroup
                                       from candidateCandidateResumeList in candidateCandidateResumeListGroup.DefaultIfEmpty()

                                       join declarationList in _commonRepositry.ApplicationDeclarationsList()
                                           on candidate.CandidateId equals declarationList.CandidateId into declarationListGroup
                                       from declarationList in declarationListGroup.DefaultIfEmpty()

                                       join questionList in _commonRepositry.ApplicationQuestionList()
                                           on candidate.CandidateId equals questionList.CandidateId into questionListGroup
                                       from questionList in questionListGroup.DefaultIfEmpty()

                                       select new GetResumeParserDetailByIdResDTO
                                       {
                                           FirstName = candidate.FirstName,
                                           LastName = candidate.LastName,
                                           Email = candidate.Email,
                                           Mobile = candidate.Mobile,
                                           CountryCode = candidate.CountryCode,
                                           ResumeFileName = candidateCandidateResumeList.FileName,
                                           ResumeFilePath = candidateCandidateResumeList.FilePath,

                                           Source = questionList.Source,
                                           NoticePeriod = questionList.NoticePeriod,
                                           ExpectedCtc = questionList.ExpectedCtc,
                                           IsTeferral = questionList.IsTeferral,
                                           ReferralName = questionList.ReferralName,

                                           AcceptedTerms = declarationList.AcceptedTerms,

                                           Skills = _commonRepositry.CandidateSkillList()
                                                       .Where(x => x.CandidateId == candidate.CandidateId)
                                                       .Select(x => x.SkillName)
                                                       .ToList(),

                                           Languages = _commonRepositry.CandidateLanguageList()
                                                       .Where(x => x.CandidateId == candidate.CandidateId)
                                                       .Select(x => x.LanguageName)
                                                       .ToList(),

                                           WorkExperienceDetail = _commonRepositry.CandidateWorkExperienceList()
                                                       .Where(x => x.CandidateId == candidate.CandidateId)
                                                       .Select(x => new GetResumeParserDetailByIdResDTO.WorkExperienceModel
                                                       {
                                                           CompanyName = x.CompanyName,
                                                           JobTitle = x.CandidateWorkExperiencesJobTitle,
                                                           StartDate = x.StartDate,
                                                           EndDate = x.EndDate,
                                                           IsCurrentCompany = x.IsCurrentCompany,
                                                           Description = x.Description
                                                       }).ToList(),

                                           Educations = _commonRepositry.CandidateEducationList()
                                                       .Where(x => x.CandidateId == candidate.CandidateId)
                                                       .Select(x => new GetResumeParserDetailByIdResDTO.EducationModel
                                                       {
                                                           CollegeName = x.CollegeName,
                                                           Degree = x.Degree,
                                                           FieldOfStudy = x.FieldOfStudy,
                                                           StartYear = x.StartYear,
                                                           EndYear = x.EndYear
                                                       }).ToList()
                                       }
                                   ).FirstOrDefaultAsync();


                if (resumeParserDetail != null)
                {
                    response.Data = resumeParserDetail;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Status = true;
                    response.Message = "Resume Parser DEtail Found Sucessfully!";
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = "Data not found!";
                }
            }
            catch { throw; }
            return response;
        }


        public async Task<CommonResponse> AddEditResumeParser(GetResumeParserDetailByIdResDTO request)
        {
            CommonResponse response = new CommonResponse();
            userId = _commonHelper.GetUserId();
            //userId = 1;
            try
            {
                var candidateExists = await _commonRepositry.CandidateList().FirstOrDefaultAsync(x => x.ResumeParserUserId == userId);
                if (candidateExists != null && !string.IsNullOrWhiteSpace(request.TabSelectedName))
                {
                    var jobApplicationDetail = await _commonRepositry.JobApplicationList().FirstOrDefaultAsync(x => x.CandidateId == candidateExists.CandidateId);

                    if (request.TabSelectedName == "CandidatePersonalInformation")
                    {

                        candidateExists.Email = request.Email;
                        candidateExists.FirstName = request.FirstName;
                        candidateExists.LastName = request.LastName;
                        candidateExists.Mobile = request.Mobile;
                        candidateExists.CountryCode = request.CountryCode;
                        candidateExists.ResumeParserUserId = userId;
                        candidateExists.CreatedDate = currentDateTime;
                        candidateExists.FinalSubmitted = true;

                        _hrmdbContext.Entry(candidateExists).State = EntityState.Modified;
                        await _hrmdbContext.SaveChangesAsync();

                        jobApplicationDetail.FinalSubmitted = true;

                        _hrmdbContext.Entry(jobApplicationDetail).State = EntityState.Modified;
                        await _hrmdbContext.SaveChangesAsync();

                    }
                    else if (request.TabSelectedName == "CandidateWorkExperinceInformation")
                    {
                        var SkillsDetail = await _commonRepositry.CandidateSkillList().Where(x => x.CandidateId == candidateExists.CandidateId).ToListAsync();
                        if (SkillsDetail.Count > 0)
                        {
                            _hrmdbContext.CandidateSkills.RemoveRange(SkillsDetail);
                            await _hrmdbContext.SaveChangesAsync();
                        }

                        if (request.Skills.Count > 0)
                        {
                            List<CandidateSkill> candidateSkillList = new List<CandidateSkill>();

                            foreach (var item in request.Skills)
                            {
                                CandidateSkill candidateSkill = new CandidateSkill();
                                candidateSkill.CandidateId = candidateExists.CandidateId;
                                candidateSkill.JobApplicationId = jobApplicationDetail.JobApplicationId;
                                candidateSkill.SkillName = item;
                                candidateSkillList.Add(candidateSkill);
                            }
                            if (candidateSkillList.Count > 0)
                            {
                                await _hrmdbContext.CandidateSkills.AddRangeAsync(candidateSkillList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }


                        var languageDetail = await _commonRepositry.CandidateLanguageList().Where(x => x.CandidateId == candidateExists.CandidateId).ToListAsync();
                        if (languageDetail.Count > 0)
                        {
                            _hrmdbContext.CandidateSkills.RemoveRange(SkillsDetail);
                            await _hrmdbContext.SaveChangesAsync();
                        }

                        if (request.Languages.Count > 0)
                        {
                            List<CandidateLanguage> candidateLanguageList = new List<CandidateLanguage>();

                            foreach (var item in request.Languages)
                            {
                                CandidateLanguage candidateLanguage = new CandidateLanguage();
                                candidateLanguage.CandidateId = candidateExists.CandidateId;
                                candidateLanguage.JobApplicationId = jobApplicationDetail.JobApplicationId;
                                candidateLanguage.LanguageName = item;
                                candidateLanguageList.Add(candidateLanguage);
                            }
                            if (candidateLanguageList.Count > 0)
                            {
                                await _hrmdbContext.CandidateLanguages.AddRangeAsync(candidateLanguageList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }

                        var workExpeinceDetail = await _commonRepositry.CandidateWorkExperienceList().Where(x => x.CandidateId == candidateExists.CandidateId).ToListAsync();

                        if (workExpeinceDetail.Count > 0)
                        {
                            _hrmdbContext.CandidateSkills.RemoveRange(SkillsDetail);
                            await _hrmdbContext.SaveChangesAsync();
                        }

                        if (request.WorkExperienceDetail.Count > 0)
                        {
                            List<CandidateWorkExperience> candidateWorkExperinceList = new List<CandidateWorkExperience>();

                            foreach (var item in request.WorkExperienceDetail)
                            {
                                CandidateWorkExperience candidateWorkExperince = new CandidateWorkExperience();
                                candidateWorkExperince.CandidateId = candidateExists.CandidateId;
                                candidateWorkExperince.JobApplicationId = jobApplicationDetail.JobApplicationId;
                                candidateWorkExperince.JobId = jobApplicationDetail.JobId;
                                candidateWorkExperince.CandidateWorkExperiencesJobTitle = item.JobTitle;
                                candidateWorkExperince.CompanyName = item.CompanyName;
                                candidateWorkExperince.StartDate = item.StartDate;
                                candidateWorkExperince.EndDate = item.EndDate;
                                candidateWorkExperince.IsCurrentCompany = item.IsCurrentCompany;
                                candidateWorkExperince.Description = item.Description;
                                candidateWorkExperinceList.Add(candidateWorkExperince);
                            }
                            if (candidateWorkExperinceList.Count > 0)
                            {
                                await _hrmdbContext.CandidateWorkExperiences.AddRangeAsync(candidateWorkExperinceList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }

                    }
                    else if (request.TabSelectedName == "CandidateEducationInformation")
                    {

                        var educationDetail = await _commonRepositry.CandidateEducationList().Where(x => x.CandidateId == candidateExists.CandidateId).ToListAsync();

                        if (educationDetail.Count > 0)
                        {
                            _hrmdbContext.CandidateEducations.RemoveRange(educationDetail);
                            await _hrmdbContext.SaveChangesAsync();
                        }
                        if (request.Educations.Count > 0)
                        {

                            List<CandidateEducation> candidateEducationList = new List<CandidateEducation>();

                            foreach (var item in request.Educations)
                            {
                                CandidateEducation candidateEducation = new CandidateEducation();
                                candidateEducation.CandidateId = candidateExists.CandidateId;
                                candidateEducation.JobApplicationId = jobApplicationDetail.JobApplicationId;
                                candidateEducation.CollegeName = item.CollegeName;
                                candidateEducation.Degree = item.Degree;
                                candidateEducation.FieldOfStudy = item.FieldOfStudy;
                                candidateEducation.StartYear = item.StartYear;
                                candidateEducation.EndYear = item.EndYear;
                                candidateEducationList.Add(candidateEducation);
                            }
                            if (candidateEducationList.Count > 0)
                            {
                                await _hrmdbContext.CandidateEducations.AddRangeAsync(candidateEducationList);
                                await _hrmdbContext.SaveChangesAsync();
                            }
                        }
                    }
                    else if (request.TabSelectedName == "OtherQuestions")
                    {
                        var otherQuestionsDetail = await _commonRepositry.ApplicationQuestionList().FirstOrDefaultAsync(x => x.CandidateId == candidateExists.CandidateId);
                        if (otherQuestionsDetail != null)
                        {
                            otherQuestionsDetail.Source = request.Source;
                            otherQuestionsDetail.ReferralName = request.ReferralName;
                            otherQuestionsDetail.IsTeferral = request.IsTeferral;
                            otherQuestionsDetail.NoticePeriod = request.NoticePeriod;
                            otherQuestionsDetail.ExpectedCtc = request.ExpectedCtc;

                            _hrmdbContext.Entry(otherQuestionsDetail).State = EntityState.Modified;
                            await _hrmdbContext.SaveChangesAsync();
                        }
                        else
                        {
                            ApplicationQuestion applicationQuestion = new ApplicationQuestion();
                            applicationQuestion.JobApplicationId = jobApplicationDetail.JobApplicationId;
                            applicationQuestion.Source = request.Source;
                            applicationQuestion.ReferralName = request.ReferralName;
                            applicationQuestion.IsTeferral = request.IsTeferral;
                            applicationQuestion.NoticePeriod = request.NoticePeriod;
                            applicationQuestion.ExpectedCtc = request.ExpectedCtc;
                            applicationQuestion.CandidateId = candidateExists.CandidateId;

                            await _hrmdbContext.ApplicationQuestions.AddAsync(applicationQuestion);
                            await _hrmdbContext.SaveChangesAsync();

                        }
                    }
                    else if (request.TabSelectedName == "TermsAndConditions")
                    {
                        var termsConditionDetail = await _commonRepositry.ApplicationDeclarationsList().FirstOrDefaultAsync(x => x.CandidateId == candidateExists.CandidateId);
                        if (termsConditionDetail != null)
                        {
                            termsConditionDetail.AcceptedTerms = request.AcceptedTerms;
                            termsConditionDetail.SubmittedDate = currentDateTime;


                            _hrmdbContext.Entry(termsConditionDetail).State = EntityState.Modified;
                            await _hrmdbContext.SaveChangesAsync();
                        }
                        else
                        {
                            ApplicationDeclaration applicationDeclaration = new ApplicationDeclaration();
                            applicationDeclaration.JobApplicationId = jobApplicationDetail.JobApplicationId;
                            applicationDeclaration.AcceptedTerms = request.AcceptedTerms;
                            applicationDeclaration.SubmittedDate = currentDateTime;
                            applicationDeclaration.CandidateId = candidateExists.CandidateId;

                            await _hrmdbContext.ApplicationDeclarations.AddAsync(applicationDeclaration);
                            await _hrmdbContext.SaveChangesAsync();

                        }
                    }


                    response.Status = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = "Data updated done!";
                }
                else
                {
                    response.Status = false;
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Something went wrong!";
                }

            }
            catch { throw; }
            return response;

        }

        public async Task<CommonResponse> RegistrationResumepArserUser(RegistrationResumepArserUserReqDTO request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                ResumeParserUser resumeParserUser = new ResumeParserUser();
                resumeParserUser.Email = request.Email;
                resumeParserUser.Password = request.Password;

                await _hrmdbContext.ResumeParserUsers.AddAsync(resumeParserUser);
                await _hrmdbContext.SaveChangesAsync();

                response.Status = true;
                response.Message = "user registered successfully!";
                response.StatusCode = HttpStatusCode.OK;
                response.Data = resumeParserUser.ResumeParserId;
            }
            catch { throw; }
            return response;
        }

    }
}

