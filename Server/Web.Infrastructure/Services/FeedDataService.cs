using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class FeedDataService : BaseService
    {
        private readonly BatchService _batchService;
        private readonly NoticeService _noticeService;
        private readonly CourseService _courseService;
        private readonly UserService _userService;

        public FeedDataService(BatchService batchService,
            NoticeService noticeService,
            CourseService courseService,
            UserService userService)
        {
            _batchService = batchService;
            _noticeService = noticeService;
            _courseService = courseService;
            _userService = userService;
        }
        public void Init()
        {
            //var allBatch = await _batchService.GetCount();
            //if (allBatch == 0)
            //{
            //    await LoadDemoBatches();
            //    await CreateCourse();
            //}

            //var notices = await _noticeService.GetTotalNoticeAsync();
            //if(notices == 0)
            //{
            //    await CreateNotice();
            //}
        }

        private async Task CreateNotice()
        {
            var intDate = CurrentTime.AddYears(-3);

            var crUsers = new List<DBUser>();
            var batches = await _batchService.GetBatchesAsync(1);
            foreach (var batch in batches)
            {
                var dbbatch = await _batchService.GetBatchAsync(batch.Id);
                var cr = dbbatch.Students.FirstOrDefault(x => x.ClassRepresentative);
                if (cr != null)
                {
                    crUsers.Add(cr);
                }
            }
            var rnd = new Random();

            var msg = string.Empty;
            var assembly = typeof(FeedDataService).Assembly;
            var name = "Student.Infrastructure.Resources.sampleText.txt";
            using var resource = assembly.GetManifestResourceStream(name);
            using (var fileStream = new StreamReader(resource))
            {
                msg = await fileStream.ReadToEndAsync();
            }

            while (intDate <= CurrentTime)
            {
                var user = crUsers[rnd.Next(0, crUsers.Count - 1)];
                var dbUser = await _userService.GetUser(user.Id);
                var notice = new Notice()
                {
                    EventDate = intDate.AddDays(rnd.Next(0, 20)),
                    CreatedOn = intDate,
                    Title = "Test Notification from " + user.Name,
                    Message = msg,
                    PostType = PostType.Notice
                };

                await _noticeService.AddNotice(notice, user, dbUser.Batch.Id);
                intDate = intDate.AddDays(1);
            }
        }

        private async Task CreateCourse()
        {
            var total = await _batchService.GetCount();
            for (var counter = 1; counter <= total; counter++)
            {
                var batches = await _batchService.GetBatchesAsync(counter);
                foreach (var batch in batches)
                {
                    await FilleCourse(batch);
                }
            }
        }

        private async Task FilleCourse(Batch batch)
        {
            Random rnd = new Random();
            var dbBatch = await _batchService.GetBatchAsync(batch.Id);
            foreach (var semester in dbBatch.Semesters)
            {
                for (int counter = 0; counter < 5; counter++)
                {
                    var course = new Course()
                    {
                        CourseCredit = counter % 2 == 0 ? 3 : 4,
                        CourseId = (semester.Id * 100 + counter).ToString(),
                        CourseName = "Test Course " + counter.ToString()
                    };
                    var dbCourse = await _courseService.AddCourse(course, semester.Id, batch.Id);

                    var lesson = new Lesson()
                    {
                        DayOfWeek = (DayOfWeek)rnd.Next(0, 6),
                        TeacherName = "Test Teacher",
                        TimeOfDay = TimeSpan.FromHours(18.5),
                    };
                    await _courseService.AddUpdateLesson(dbCourse.Id, lesson);
                }
            }
        }

        private async Task LoadDemoBatches()
        {
            var dbUsers = new List<DBUser>();
            int batchNo = 19;
            int lastBacth = 16;
            var assembly = typeof(FeedDataService).Assembly;
            var name = "Student.Infrastructure.Resources.template.csv";

            using var resource = assembly.GetManifestResourceStream(name);
            using (var fileStream = new StreamReader(resource))
            {
                string line = string.Empty;
                while ((line = await fileStream.ReadLineAsync()) != null)
                {
                    var data = line.Split(',');
                    var dbUser = new DBUser()
                    {
                        Name = data[0],
                        UserName = data[1],
                        Roll = int.Parse(data[1]),
                        Email = data[2],
                        PhoneNumber = data[3]
                    };
                    dbUsers.Add(dbUser);
                    if (dbUsers.Count >= 40)
                    {
                        await CreateBatch(batchNo, dbUsers);
                        dbUsers.Clear();
                        batchNo--;
                        if (batchNo <= lastBacth)
                        {
                            break;
                        }
                    }
                }
            };
        }

        private async Task CreateBatch(int batchNo, List<DBUser> dbUsers)
        {
            var batchType = (batchNo % 2 == 0) ? "MIT " : "PGDIT ";
            var batch = new Batch()
            {
                StartsOn = new DateTime(2000 + batchNo, 1, 1),
                NumberOfSemester = 4,
                SemesterDuration = 4,
                Name = batchType + batchNo.ToString(),
            };
            var dBbatch = await _batchService.AddBatch(batch);
            dbUsers.FirstOrDefault().ClassRepresentative = true;
            await _userService.AddStudents(dbUsers, dBbatch.Id);
        }
    }
}
