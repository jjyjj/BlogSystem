using System;

namespace BlogSystem.Dto
{
    public class UserInformationDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }

        public string PassWord { get; set; }
        public string Motto { get; set; }
        public int FansCount { get; set; }

        public int FocusCount { get; set; }

        public string SiteName { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
