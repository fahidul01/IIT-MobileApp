﻿using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Student.Infrasructure.DBModel;
using Student.Infrastructure.AppServices;
using Student.Infrastructure.DBModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class UserService : BaseService
    {
        #region Init
        private readonly UserManager<IdentityDBUser> _usermanager;
        private readonly StudentDBContext _db;
        private readonly IEmailSender _emailSender;

        public UserService(
            UserManager<IdentityDBUser> userManager,
            StudentDBContext studentDB,
            IEmailSender emailSender)
        {
            _usermanager = userManager;
            _db = studentDB;
            _emailSender = emailSender;
        }
        #endregion

        #region Insert

        public async Task<ActionResponse> Update(string userId, DBUser user)
        {
            var dbUser = await _db.DBUsers
                                  .FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbUser == null || dbUser.Id != userId)
            {
                return new ActionResponse(false, "Invalid User information");
            }
            else
            {
                dbUser.UpdateUser(user);
                _db.Entry(dbUser).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return new ActionResponse(true);
            }
        }
        internal async Task<List<DBUser>> AddStudents(IList<DBUser> students, int batchID)
        {
            var users = new List<DBUser>();
            var batch = await _db.Batches.FindAsync(batchID);
            foreach (var student in students)
            {
                var oldUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == student.UserName);
                if (oldUser != null)
                {
                    continue;
                }

                student.Batch = batch;
                student.PhoneNumberConfirmed = false;
                student.Role = AppConstants.Student;

                var idbUser = IdentityDBUser.Create(student);
                var res = await _usermanager.CreateAsync(idbUser);
                if (res.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(idbUser, AppConstants.Student);
                }
            }
            return users;
        }
        public async Task<ActionResponse> AddStudent(int batchId, string roll, string name, string email, string phone)
        {
            var batch = await _db.Batches.FirstOrDefaultAsync(x => x.Id == batchId);
            if (batch == null)
            {
                return new ActionResponse(false, "Invalid Batch");
            }

            var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == roll);
            if (user != null)
            {
                return new ActionResponse(false, "Roll already exists");
            }
            else
            {
                int.TryParse(roll, out int studentRoll);
                var student = new DBUser()
                {
                    Batch = batch,
                    Email = email,
                    EnrolledIn = CurrentTime,
                    Name = name,
                    UserName = roll,
                    Roll = studentRoll,
                    Role = AppConstants.Student,
                    PhoneNumber = phone,
                    PhoneNumberConfirmed = false
                };
                var idUser = IdentityDBUser.Create(student);
                var res = await _usermanager.CreateAsync(idUser);
                if (res.Succeeded)
                {
                    var mRes = await _usermanager.AddToRoleAsync(idUser, AppConstants.Student);
                    if (mRes.Succeeded)
                    {
                        return new ActionResponse(true);
                    }

                    var msg = EmailMessageCreator.CreateInvitation(phone);
                    await _emailSender.SendEmailAsync(student.Email, "Password Recover", msg);
                }
                return new ActionResponse(false, "Failed to create User");
            }
        }
        #endregion

        #region Data
        public async Task<IdentityDBUser> GetIdentityUserByDBUserId(string userId)
        {
            return await _db.Users
                            .FirstOrDefaultAsync(x => x.DBUser.Id == userId);
        }

        public async Task<DBUser> GetUserByName(string userName)
        {
            return await _db.DBUsers.FirstOrDefaultAsync(x=>x.UserName == userName);
        }

        public async Task<Batch> GetBatch(string userId)
        {
            var user = await _db.DBUsers.Include(x => x.Batch)
                                      .FirstOrDefaultAsync(x => x.Id == userId);
            return user?.Batch;
        }

        public async Task<DBUser> GetUser(string userId)
        {
            var user = await _db.DBUsers
                                .FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<DBUser> MakeCR(string userId)
        {
            var user = await _db.DBUsers.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }
            else
            {
                user.ClassRepresentative = true;
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return await GetUser(userId);
            }
        }

        public async Task<DBUser> RemoveCR(string id)
        {
            var user = await _db.DBUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            else
            {
                user.ClassRepresentative = false;
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return await GetUser(id);
            }
        }

        public async Task<List<DBUser>> GetCurrentCr()
        {
            var users = await _db.DBUsers
                                 .Where(x => x.Batch.EndsOn >= CurrentTime &&
                                             x.ClassRepresentative)
                                 .OrderByDescending(x => x.Batch.Id)
                                 .Include(x => x.Batch)
                                 .ToListAsync();
            return users;
        }

        public async Task<List<DBUser>> SearchStudent(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var res = await _db.DBUsers
                                   .Where(x => x.Role == AppConstants.Student)
                                   .OrderByDescending(x => x.EnrolledIn)
                                   .Take(50)
                                   .ToListAsync();
                return res;
            }
            else
            {
                var res = await _db.DBUsers
                                   .Where(x => x.Role == AppConstants.Student &&
                                                (x.UserName == value ||
                                                 EF.Functions.Like(x.Name, $"%{value}%")))
                                           .OrderByDescending(x => x.EnrolledIn)
                                           .Take(100)
                                           .ToListAsync();
                return res;
            }
        }
        #endregion

        #region Authorize
        public async Task<bool> AuthorizeLesson(string userId, int lessonId)
        {
            var lesson = await _db.Lessons.Include(x => x.Course)
                                          .FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null)
            {
                return false;
            }
            else
            {
                return await AuthorizeCourse(userId, lesson.Course.Id);
            }
        }

        public async Task<bool> AuthorizeSemester(string userId, int semesterId)
        {
            var user = await _db.DBUsers.Include(x => x.Batch)
                                        .FirstOrDefaultAsync(x => x.Id == userId);
            if (user.Role == AppConstants.Admin)
            {
                return true;
            }
            else
            {
                var semester = await _db.Semesters.Include(m => m.Batch)
                                      .FirstOrDefaultAsync(x => x.Id == semesterId);
                return semester?.Batch.Id == user.Batch?.Id;
            }
        }

        public async Task<bool> AuthorizeCourse(string userId, int courseId)
        {
            var user = await _db.DBUsers.Include(x => x.Batch)
                                      .FirstOrDefaultAsync(x => x.Id == userId);
            if (user.Role == AppConstants.Admin)
            {
                return true;
            }
            else
            {
                var course = await _db.Courses.Include(m => m.Semester)
                                      .ThenInclude(m => m.Batch)
                                      .FirstOrDefaultAsync(x => x.Id == courseId);
                return course.Semester.Batch.Id == user.Batch?.Id;
            }
        }
        #endregion

        #region Verification
        public async Task<ActionResponse> VerifyPhoneNo(string rollNo, string phoneNo)
        {
            rollNo = rollNo.Trim();
            phoneNo = phoneNo.Replace("+88", "").Trim();
            var res = await _db.Users.FirstOrDefaultAsync(x => x.UserName == rollNo);
            if (res == null)
            {
                return new ActionResponse(false, "Invalid Roll number/Username");
            }
            else if (res.PhoneNumber != phoneNo)
            {
                return new ActionResponse(false, "Invalid Mobile No");
            }
            else if (res.PhoneNumberConfirmed)
            {
                return new ActionResponse(true, "Phone number already confirmed");
            }
            else
            {
                return new ActionResponse(true, "Phone Number verified");
            }
        }

        public async Task<ActionResponse> ConfirmRegistration(string rollNo, string phoneNo, string password)
        {
            rollNo = rollNo.Trim();
            phoneNo = phoneNo.Replace("+88", "").Trim();
            var dbUser = await _db.DBUsers.FirstOrDefaultAsync(x => x.UserName == rollNo);
            if (dbUser == null)
            {
                return new ActionResponse(false, "Invalid Roll number/Username");
            }
            else if (dbUser.PhoneNumber != phoneNo)
            {
                return new ActionResponse(false, "Invalid Mobile Number");
            }
            else
            {
                var identityUser = await GetIdentityUserByDBUserId(dbUser.Id);
                await _usermanager.RemovePasswordAsync(identityUser);
                var resetRes = await _usermanager.AddPasswordAsync(identityUser, password);
                if (resetRes.Succeeded)
                {
                    identityUser.PhoneNumberConfirmed = true;
                    dbUser.PhoneNumberConfirmed = true;
                    _db.Entry(dbUser).State = EntityState.Modified;
                    _db.Entry(identityUser).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return new ActionResponse(true);
                }
                else
                {
                    return new ActionResponse(false, resetRes.Errors.Select(x => x.Description));
                }
            }
        }

        public async Task<List<DBUser>> UploadCSVStudents(string filePath, int batchId)
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

        public async Task<ActionResponse> CheckPhoneNumber(string phoneNo)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNo);
            return new ActionResponse(user != null);
        }

        public async Task<ActionResponse> RecoverPasswordPhoneNo(string rollNo, string phoneNo, string password)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNo);
            if (dbUser == null)
            {
                return new ActionResponse(false, "Invalid Mobile No. Contact Admin");
            }

            var token = await _usermanager.GeneratePasswordResetTokenAsync(dbUser);
            var resetRes = await _usermanager.ResetPasswordAsync(dbUser, token, password);


#if DEBUG

#else
            var msg = EmailMessageCreator.CreatePasswordRecovery();
            var res = await _emailSender.SendEmailAsync(dbUser.Email, "Password Recover", msg);
#endif
            return new ActionResponse(true);
        }

        #endregion


    }
}
