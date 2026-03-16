using Application.DTO.RequestDTO.ResumeParser;
using Application.DTO.ResponseDTO.ResumeParser;
using Helper;

namespace Application.Interface
{
    public interface IResumeParser
    {
        public Task<CommonResponse> AddDEtailByResumeParser(AddDEtailByResumeParserReqDTO request);

        public Task<CommonResponse> GetResumeParserDetailById(int id);
        public Task<CommonResponse> AddEditResumeParser(GetResumeParserDetailByIdResDTO request);
        public Task<CommonResponse> RegistrationResumepArserUser(RegistrationResumepArserUserReqDTO request);
    }
}
