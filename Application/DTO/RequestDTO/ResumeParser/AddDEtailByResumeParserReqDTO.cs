using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.RequestDTO.ResumeParser
{
    public class AddDEtailByResumeParserReqDTO
    {
        public dynamic FileUplaod { get; set; }
        public int JobId { get; set; }
        //public int? CandidateId { get; set; }
    }
}
