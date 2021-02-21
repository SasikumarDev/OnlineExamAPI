using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class Category
    {
        public Category()
        {
            Questions = new HashSet<Question>();
        }

        public Guid Cid { get; set; }
        public string Cdesc { get; set; }
        public int CqstType { get; set; }
        public int CnoOfQst { get; set; }
        public int CdefaultScoreorQst { get; set; }
        public int CtotalQst { get; set; }
        public int Cstatus { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
