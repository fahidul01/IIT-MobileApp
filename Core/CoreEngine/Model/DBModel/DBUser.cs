using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CoreEngine.Model.DBModel
{
    public class DBUser : IdentityUser
    {
        public virtual Batch Batch { get; set; }
        public virtual ICollection<CourseMaterial> CourseMaterials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static User FromDBUser(DBUser dBUser, string password)
        {
            return new User()
            {
                UserName = dBUser.UserName,
                Password = password,
                Email = dBUser.Email,
                FirstName = dBUser.FirstName,
                LastName = dBUser.LastName
            };
        }
    }
}
