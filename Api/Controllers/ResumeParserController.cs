using Api.Model.RequestModel.ResumeParser;
using Api.Model.ResponseModel.ResumeParser;
using Application.DTO.RequestDTO.ResumeParser;
using Application.DTO.ResponseDTO.ResumeParser;
using Application.Interface;
using Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResumeParserController : ControllerBase
    {
        private readonly IResumeParser _iResumeParser;
        public ResumeParserController(IResumeParser iResumeParser)
        {
            _iResumeParser = iResumeParser;
        }

        
        [HttpPost("AddDetailByResumeParser")]
        public async Task<CommonResponse> AddDEtailByResumeParser([FromForm] AddDEtailByResumeParserRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
               response = await _iResumeParser.AddDEtailByResumeParser(request.Adapt<AddDEtailByResumeParserReqDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpGet("GetResumeParserDetailById")]
        public async Task<CommonResponse> GetResumeParserDetailById(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iResumeParser.GetResumeParserDetailById(id);
                GetResumeParserDetailByIdResDTO resDTO = response.Data;
                response.Data = resDTO.Adapt<GetResumeParserDetailByIdResponseModel>();
            }
            catch (Exception ex) { throw ex; }
            return response;
        }


        [HttpPut("AddEditResumeParser")]
        public async Task<CommonResponse> AddEditResumeParser(GetResumeParserDetailByIdResponseModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iResumeParser.AddEditResumeParser(request.Adapt<GetResumeParserDetailByIdResDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        [HttpPost("RegistrationResumepArserUser")]
        [AllowAnonymous]
        public async Task<CommonResponse> RegistrationResumepArserUser(RegistrationResumepArserUserRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _iResumeParser.RegistrationResumepArserUser(request.Adapt<RegistrationResumepArserUserReqDTO>());
            }
            catch (Exception ex) { throw ex; }
            return response;
        }
    }
}
