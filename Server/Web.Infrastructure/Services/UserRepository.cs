using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class UserRepository
    {
        readonly UserManager<DBUser> _usermanager;
        readonly StudentDBContext _db;
        public UserRepository(UserManager<DBUser> userManager, StudentDBContext studentDB)
        {
            _usermanager = userManager;
            _db = studentDB;
        }

        public async Task<List<User>> AddStudents(IList<DBUser> students, int batchID)
        {
            var users = new List<User>();
            var batch = await _db.Batches.FindAsync(batchID);
            foreach (var student in students)
            {
                var password = CryptoService.GenerateRandomPassword();
                student.Batch = batch;
                var res = await _usermanager.CreateAsync(student);
                if (res.Succeeded)
                {
                    users.Add(User.FromDBUser(student, password));
                    await _usermanager.AddToRoleAsync(student, AppConstants.Student);
                }
            }
            return users;
        }
    }
}
