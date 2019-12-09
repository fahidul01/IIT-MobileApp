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
using Web.Infrastructure.AppServices;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class UserService : BaseService
    {
        private readonly UserManager<DBUser> _usermanager;
        private readonly StudentDBContext _db;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<DBUser> userManager, StudentDBContext studentDB, IEmailSender emailSender)
        {
            _usermanager = userManager;
            _db = studentDB;
            _emailSender = emailSender;
        }

        internal async Task<List<User>> AddStudents(IList<DBUser> students, int batchID)
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

        public async Task<Batch> GetBatch(string userId)
        {
            var user = await _db.Users.Include(x => x.Batch)
                                      .FirstOrDefaultAsync(x => x.Id == userId);
            return user?.Batch;
        }

        public async Task<User> Update(User user)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbUser != null)
            {
                dbUser.UpdateUser(user);
                _db.Entry(dbUser).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return await GetStudent(dbUser.Id);
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetStudent(string id)
        {
            var user = await _db.Users.Include(x => x.Batch)
                                      .FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                var terminalUser = User.FromDBUser(user, "");
                terminalUser.Batch = user.Batch;
                var courses = await _db.StudentCourses.Where(x => x.Student.Id == user.Id)
                                                      .Include(x => x.Course)
                                                      .Include(m => m.Grade)
                                                      .ToListAsync();
                terminalUser.Courses = courses;
                return terminalUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<ActionResponse> AddStudent(int batchId, string roll, string name, string email, string phone)
        {
            var batch = await _db.Batches.FirstOrDefaultAsync(x => x.Id == batchId);
            if (batch == null) return new ActionResponse(false, "Invalid Batch");
            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == roll);
            if (user != null) return new ActionResponse(false, "Roll already exists");
            else
            {
                var password = CryptoService.GenerateRandomPassword();
                int.TryParse(roll, out int studentRoll);
                var student = new DBUser()
                {
                    Batch = batch,
                    Email = email,
                    EnrolledIn = CurrentTime,
                    Name = name,
                    UserName = roll,
                    Roll = studentRoll,
                    UserRole = AppConstants.Student
                };
                var res = await _usermanager.CreateAsync(student);
                if (res.Succeeded)
                {
                    var mRes = await _usermanager.AddToRoleAsync(student, AppConstants.Student);
                    if (mRes.Succeeded) return new ActionResponse(true);
                }
                return new ActionResponse(false, "Failed to create User");
            }
        }

        public async Task<List<User>> GetAllStudent(DBUser dbUser)
        {
            var batch = await _db.Batches.Include(x => x.Students)
                                   .FirstOrDefaultAsync(x => x.Students.Any(m => m.Id == dbUser.Id));
            batch?.LoadUsers();
            return batch?.ExternalUsers;
        }

        public async Task<User> MakeCR(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            else
            {
                user.ClassRepresentative = true;
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return await GetStudent(id);
            }
        }

        public async Task<User> RemoveCR(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            else
            {
                user.ClassRepresentative = false;
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return await GetStudent(id);
            }
        }

        public async Task<List<User>> GetCurrentCr()
        {
            var users = await _db.Users.Where(x => x.Batch.EndsOn >= CurrentTime &&
                                                   x.ClassRepresentative)
                                 .OrderByDescending(x => x.Batch.Id)
                                 .Include(x => x.Batch)
                                 .ToListAsync();
            var userList = new List<User>();
            foreach (var item in users)
            {
                var user = User.FromDBUser(item, "");
                user.Batch = item.Batch;
                userList.Add(user);
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
                    var user = User.FromDBUser(item, "");
                    users.Add(user);
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

        public async Task<bool> RecoverPassword(string id)  // Supports username too
        {
            var user = await _usermanager.FindByIdAsync(id);
            if (user == null)
            {
                user = await _usermanager.FindByNameAsync(id);
            }
            if (user == null)
            {
                return false;
            }
            else
            {
                return await ResetPassword(user);
            }
        }

        private async Task<bool> ResetPassword(DBUser dBUser)
        {
            try
            {
                var password = CryptoService.GenerateRandomPassword();
                var token = await _usermanager.GeneratePasswordResetTokenAsync(dBUser);
                var msg = new EmailMessageCreator().CreatePasswordRecovery(password);
                var res = await _emailSender.SendEmailAsync(dBUser.Email, "Password Recover", msg);
                await _usermanager.ResetPasswordAsync(dBUser, token, password);
                return true;
            }
            catch (Exception ex)
            {
                LogEngine.Error(ex);
                return false;
            }
        }
    }
}
