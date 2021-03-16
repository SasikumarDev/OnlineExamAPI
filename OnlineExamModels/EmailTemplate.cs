using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class EmailTemplate
    {
        public Guid Etid { get; set; }
        public string Ettemplate { get; set; }
        public string Etsubject { get; set; }
        public DateTime Etcreatedon { get; set; }
    }
}
