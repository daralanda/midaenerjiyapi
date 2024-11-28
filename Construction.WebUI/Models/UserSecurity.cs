using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Construction.WebUI.Models
{
    public class UserSecurity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public int LanguageId { get; set; }
    }
}