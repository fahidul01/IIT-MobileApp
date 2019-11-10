using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models.Web;

namespace Web.Areas.Admin.ViewModels
{
    public class CreateNoticeViewModel
    {
        public int Id { get; set; }
        public SelectList BatchList { get; private set; }
        public int? BatchId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime EventDate { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png",".pdf",".xls",".xlxs",".docx" })]
        public IFormFileCollection FormFiles { get; set; }
        public CreateNoticeViewModel(Notice notice, List<Batch> batches)
        {
            Id = notice.Id;
            BatchId = notice.Batch?.Id;
            Title = notice.Title;
            Message = notice.Message;
            EventDate = notice.EventDate;
            BatchList = new SelectList(batches, nameof(Batch.Id), nameof(Batch.Name));
        }

        public CreateNoticeViewModel(List<Batch> batches)
        {
            BatchList = new SelectList(batches, nameof(Batch.Id), nameof(Batch.Name));
            EventDate = DateTime.UtcNow.AddHours(6);
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
