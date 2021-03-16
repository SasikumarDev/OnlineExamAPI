using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class Interviewee
    {
        public Interviewee()
        {
            IntervieweeTests = new HashSet<IntervieweeTest>();
        }

        public Guid Iid { get; set; }
        public string Ifname { get; set; }
        public string Ilname { get; set; }
        public string IemailId { get; set; }
        public DateTime Idob { get; set; }
        public string Imobileno { get; set; }
        public bool IattentPreviousTest { get; set; }

        public virtual ICollection<IntervieweeTest> IntervieweeTests { get; set; }
    }
}
