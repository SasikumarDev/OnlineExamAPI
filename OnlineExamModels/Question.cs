using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class Question
    {
        public Question()
        {
            Choices = new HashSet<Choice>();
        }

        public Guid Qid { get; set; }
        public Guid? Qcid { get; set; }
        public string Qtext { get; set; }
        public int Qstatus { get; set; }
        public DateTime QcreatedOn { get; set; }
        public DateTime QupdateOn { get; set; }

        public virtual Category Qc { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
    }
}
