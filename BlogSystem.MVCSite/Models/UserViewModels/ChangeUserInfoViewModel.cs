using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models.UserViewModels
{
    public class ChangeUserInfoViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string PassWord { get; set; }
        public string ImagePath { get; set; }
        public string SiteName { get; set; }
        
    }
}