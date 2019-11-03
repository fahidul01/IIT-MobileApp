using Microsoft.AspNetCore.Identity;
using System;

namespace CoreEngine.Model.DBModel
{
    public class DBUser : IdentityUser
    {
        public virtual Batch Batch { get; set; }
        public string Name { get; set; }
        public bool ClassRepresentative { get; set; }
        public int Roll { get; set; }
        public string UserRole { get; set; }
        public DateTime EnrolledIn { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCR { get; set; }
        public string ClassRepresentative => IsCR ? "CR" : "";
        public int Roll { get; set; }

        public static User FromDBUser(DBUser dBUser, string password)
        {
            return new User()
            {
                UserName = dBUser.UserName,
                Password = password,
                Email = dBUser.Email,
                Name = dBUser.Name,
                PhoneNumber = dBUser.PhoneNumber,
                Roll = dBUser.Roll,
                IsCR = dBUser.ClassRepresentative
            };
        }
    }
}
