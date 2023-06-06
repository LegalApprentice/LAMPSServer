using System.ComponentModel.DataAnnotations;

namespace LAMPSServer.Models
{
    public class Login
    {
        public string password { get; set; }
        public string username { get; set; }
    }

    public class UserData
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }


        [Key]
        public string email { get; set; }
    }

    public class UserRegister : UserData
    {
        public string password { get; set; }
        public string leader { get; set; }
    }

    public class UserModel : UserData
    {
        public string token { get; set; }
    }

    public class UserEntity : UserData
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}