using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Users
    {
        [Key]
        public int userID { get; set; }
        public string userName { get; set; }
        public string userSurName { get; set; }
        public string userPassword { get; set; }
        public string userMail { get; set; }
        public DateTime? userLogin { get; set; }
        public string userProfileImage { get; set; }

    }
}
