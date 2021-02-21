using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class Choice
    {
        public Guid Cid { get; set; }
        public Guid? Qid { get; set; }
        public string Ctext { get; set; }
        public int Ciscrt { get; set; }
        public int Cstatus { get; set; }
        public DateTime CcreatedOn { get; set; }
        public DateTime CupdateOn { get; set; }

        public virtual Question QidNavigation { get; set; }
    }
}
