using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineExamAPI.OnlineExamModels
{
    public partial class AdminUser
    {
        public Guid AdminUsId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdateOn { get; set; }
        public string Apassword { get; set; }
        public string EmailId { get; set; }
    }
}
