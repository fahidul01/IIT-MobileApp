using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class UserService : BaseService
    {
        readonly UserManager<DBUser> _usermanager;
        readonly StudentDBContext _db;
        public UserService(UserManager<DBUser> userManager, StudentDBContext studentDB)
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
                var oldUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == student.UserName);
                if (oldUser != null) continue;

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

        public async Task<List<User>> UploadCSVStudents(string filePath, int batchId)
        {
            var students = new List<DBUser>();
            using var stream = File.OpenRead(filePath);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var splitter = line.Split(',');
                var roll = int.Parse(splitter[0]);
                var name = splitter[1];
                var email = splitter[2];
                var phone = splitter[3];
                var student = new DBUser()
                {
                    Roll = roll,
                    UserName = roll.ToString(),
                    Email = email,
                    Name = name,
                    PhoneNumber = phone
                };
                students.Add(student);
            }
            return await AddStudents(students, batchId);
        }
    }
}
