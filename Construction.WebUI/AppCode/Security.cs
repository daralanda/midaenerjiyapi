using Construction.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Construction.WebUI.AppCode
{
    public class Security
    {
        private readonly List<UserSecurity> userSecurityList = new List<UserSecurity>();

        public Dictionary<int, UserSecurity> UserSecurities = null;
        public Security()
        {
            UserSecurities = new Dictionary<int, UserSecurity>();
        }

        public void UserSecurityClear()
        {
            Security security = new Security();
            HttpContext.Current.Session["UserSecurity"] = null;
        }
        private void Add(UserSecurity item)
        {
            UserSecurities.Clear();
            UserSecurities.Add(0, item);
        }
        public bool UserSecurityAdd(UserSecurity item)
        {
            try
            {
                Security security = new Security();
                security.Add(item);
                HttpContext.Current.Session["UserSecurity"] = security;
                return true;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                return false;
            }
        }
        public bool IsTrue;
        public bool UserControl()
        {
            IsTrue = false;
            Security xSecurity = (Security)HttpContext.Current.Session["UserSecurity"];
            if (xSecurity != null)
            {
                IsTrue = true;
            }
            return IsTrue;
        }
        public Security SecurityRead()
        {
            if ((Security)HttpContext.Current.Session["UserSecurity"] != null)
            {
                Security xSecurity = (Security)HttpContext.Current.Session["UserSecurity"];
                return xSecurity;
            }
            else
            {
                return null;
            }
        }
    }
}