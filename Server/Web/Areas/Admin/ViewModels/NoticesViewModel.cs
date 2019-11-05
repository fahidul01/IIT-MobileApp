using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Web.Areas.Admin.ViewModels
{
    public class CreateNoticeViewModel
    {
        public SelectList BatchList { get; private set; }
        public int BatchId { get; set; }
        public bool IsAllBatch { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime EventDate { get; set; } = DateTime.Now;
        public PostType PostType { get; internal set; }

        public CreateNoticeViewModel(List<Batch> batches)
        {
            BatchList = new SelectList(batches, nameof(Batch.Id), nameof(Batch.Name), BatchId);
        }
        public CreateNoticeViewModel()
        {

        }
    }

    public class IndexNoticeViewModel
    {
        public List<Notice> UpComingNotices { get; private set; }
        public List<Notice> RecentNotices { get; private set; }

        public IndexNoticeViewModel(List<Notice> uNotices, List<Notice> rNotices)
        {
            UpComingNotices = uNotices;
            RecentNotices = rNotices;
        }
    }
}
