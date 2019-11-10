using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class NoticeService : BaseService
    {
        private readonly StudentDBContext _db;

        public NoticeService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
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

        public async Task<bool> AddUpdateNotice(Notice post, string user)
        {
            var dbUser = await _db.Users
                                  .Include(m => m.Batch)
                                  .FirstOrDefaultAsync(x => x.UserRole == AppConstants.Student &&
                                                            x.UserName == user);
            if (dbUser == null) return false;
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
                return true;
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
            if (notice == null) return false;
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
