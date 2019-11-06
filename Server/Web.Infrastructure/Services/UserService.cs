using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class UserService : BaseService
    {
        private readonly UserManager<DBUser> _usermanager;
        private readonly StudentDBContext _db;
        public UserService(UserManager<DBUser> userManager, StudentDBContext studentDB)
        {
            _usermanager = userManager;
            _db = studentDB;
        }

        private async Task<List<User>> AddStudents(IList<DBUser> students, int batchID)
        {
            var users = new List<User>();
            var batch = await _db.Batches.FindAsync(batchID);
            foreach (var student in students)
            {
                var oldUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == student.UserName);
                if (oldUser != null)
                {
                    continue;
                }

                var password = CryptoService.GenerateRandomPassword();
                student.Batch = batch;
                student.UserRole = AppConstants.Student;
                var res = await _usermanager.CreateAsync(student);
                if (res.Succeeded)
                {
                    users.Add(User.FromDBUser(student, password));
                    await _usermanager.AddToRoleAsync(student, AppConstants.Student);
                }
            }
            return users;
        }

        public async Task<List<User>> GetCurrentCr()
        {
            var users = await _db.Users.Where(x => x.Batch.EndsOn <= DateTime.Now &&
                                             x.ClassRepresentative)
                                 .OrderByDescending(x => x.Batch.Id)
                                 .Include(x => x.Batch)
                                 .ToListAsync();
            var userList = new List<User>();
            foreach (var item in users)
            {
                userList.Add(User.FromDBUser(item, ""));
            }
            return userList;
        }

        public async Task<List<User>> SearchStudent(string value)
        {
            var users = new List<User>();
            if (string.IsNullOrWhiteSpace(value))
            {
                var res = await _db.Users.Where(x => x.UserRole == AppConstants.Student)
                                        .OrderByDescending(x => x.EnrolledIn)
                                        .Take(50)
                                        .ToListAsync();
                foreach (var item in res)
                {
                    users.Add(User.FromDBUser(item, ""));
                }
                return users;
            }
            else
            {
                var res = await _db.Users.Where(x => x.UserRole == AppConstants.Student &&
                                                (x.UserName == value ||
                                                 EF.Functions.Like(x.Name, $"%{value}%")))
                                           .OrderByDescending(x => x.EnrolledIn)
                                           .Take(100)
                                           .ToListAsync();
                foreach (var item in res)
                {
                    users.Add(User.FromDBUser(item, ""));
                }
                return users;
            }
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
