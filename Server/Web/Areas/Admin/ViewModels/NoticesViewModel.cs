using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels
{
    public class CreateNoticeViewModel
    {
        public int Id { get; set; }
        public SelectList BatchList { get; private set; }
        public int? BatchId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime EventDate { get; set; } = DateTime.Now;
        [FileExtensions(Extensions="jpg,jpeg,png,pdf")]
        [Display(Name = "Attach File")]
        public IFormFileCollection FormFiles { get; set; }
        public CreateNoticeViewModel(int id, List<Batch> batches)
        {
            Id = id;
            BatchList = new SelectList(batches, nameof(Batch.Id), nameof(Batch.Name));
        }

        public CreateNoticeViewModel(List<Batch> batches)
        {
            BatchList = new SelectList(batches, nameof(Batch.Id), nameof(Batch.Name));
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
