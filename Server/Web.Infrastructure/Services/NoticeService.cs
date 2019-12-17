using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Student.Infrastructure.AppServices;
using Student.Infrastructure.DBModel;

namespace Student.Infrastructure.Services
{
    public class NoticeService : BaseService
    {
        private readonly StudentDBContext _db;
        private readonly INotificationService _notificationService;

        public NoticeService(StudentDBContext studentDBContext, INotificationService notificationService)
        {
            _db = studentDBContext;
            _notificationService = notificationService;
        }

        public async Task<int> GetTotalNoticeAsync()
        {
            return await _db.Notices.CountAsync();
        }

        public async Task<bool> AddNotice(Notice notice, DBUser dBUser, int batchId)
        {
            if (notice.Id == 0 && dBUser != null)
            {
                if (batchId != 0)
                {
                    notice.Batch = await _db.Batches.FindAsync(batchId);
                    _notificationService.SendNotification(notice.Batch.Name, notice.Title, notice.Message);
                }
                notice.Owner = dBUser;
                _db.Notices.Add(notice);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ActionResponse> AddUpdateNotice(Notice post, string userId)
        {
            var dbUser = await _db.Users
                                  .Include(m => m.Batch)
                                  .FirstOrDefaultAsync(x => x.UserRole == AppConstants.Student &&
                                                            x.Id == userId);
            if (dbUser == null)
            {
                return new ActionResponse(false,"Invalid User");
            }
            else
            {
                post.Owner = dbUser;
                post.Batch = dbUser.Batch;
                if (post.Id == 0)
                {
                    _db.Entry(post).State = EntityState.Added;
                }
                else
                {
                    _db.Entry(post).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
                return new ActionResponse(true,"Successfully created the notice");
            }
        }

        public async Task<bool> UpdateNotice(Notice notice)
        {
            var dbNotice = await _db.Notices.FindAsync(notice);
            if (dbNotice == null)
            {
                return false;
            }
            else
            {
                _db.Entry(notice).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
        }



        public async Task<List<Notice>> GetUpcomingEvents()
        {
            var nextWeek = CurrentTime.AddDays(7);
            var notices = await _db.Notices.Where(x => x.EventDate > CurrentTime &&
                                                     x.EventDate <= nextWeek)
                                           .Include(m => m.Batch)
                                           .OrderBy(x => x.EventDate)
                                           .ToListAsync();
            return notices;
        }

        public async Task<List<Notice>> GetRecentNotice(int page, int itemPerPage = 20)
        {
            var notices = await _db.Notices.OrderByDescending(m => m.CreatedOn)
                                           .Skip((page - 1) * itemPerPage)
                                           .Take(itemPerPage)
                                           .Include(m => m.Batch)
                                           .ToListAsync();
            return notices;
        }

        public async Task<bool> Delete(int id)
        {
            var notice = await _db.Notices.Include(x => x.Batch)
                                          .FirstOrDefaultAsync(x => x.Id == id);
            if (notice == null)
            {
                return false;
            }
            else
            {
                _db.Entry(notice).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Notice> GetNotice(int id)
        {
            var notice = await _db.Notices
                                  .Include(x => x.Batch)
                                  .Include(x => x.DBFiles)
                                  .Include(x => x.Owner)
                                  .FirstOrDefaultAsync(x => x.Id == id);
            return notice;
        }
    }
}
