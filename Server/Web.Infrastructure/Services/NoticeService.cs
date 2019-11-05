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
            var nextWeek = DateTime.Now.AddDays(7);
            var notices = await _db.Notices.Where(x => x.EventDate > DateTime.Now &&
                                                     x.EventDate <= nextWeek)
                                           .ToListAsync();
            return notices;
        }

        public async Task<List<Notice>> GetRecentNotice(int page, int itemPerPage = 20)
        {
            var notices = await _db.Notices.OrderByDescending(m => m.CreatedOn)
                                           .Skip((page - 1) * itemPerPage)
                                           .Take(itemPerPage)
                                           .ToListAsync();
            return notices;
        }
    }
}
