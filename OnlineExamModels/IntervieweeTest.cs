using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class IntervieweeTest
    {
        public Guid Itid { get; set; }
        public Guid? Itiid { get; set; }
        public Guid? ItqstId { get; set; }
        public Guid? ItchoiceId { get; set; }
        public bool? ItiscorrectAns { get; set; }

        public virtual Choice Itchoice { get; set; }
        public virtual Interviewee Iti { get; set; }
        public virtual Question Itqst { get; set; }
    }
}
