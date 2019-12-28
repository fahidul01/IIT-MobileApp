using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Mobile.Core.Engines.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.ViewModels
{
    public class CalenderViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        public List<Notice> Notices { get; set; }
        public DateTime SelectedDate { get; set; }
        public ICalenderHelper CalenderHelper { get; }
        public int Year { get; set; } = 2020;
        int _month=1;
        public int Month
        {
            get => _month;
            set
            {
                _month = value;
                LoadInformation(_month);
            }
        }
        public CalenderViewModel(INoticeHandler noticeHandler,ICalenderHelper calenderHelper)
        {
            _noticeHandler = noticeHandler;
            SelectedDate = DateTime.Now;
            CalenderHelper = calenderHelper;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            CalenderHelper.Clear();
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
           // LoadInformation(DateTime.Today.Year, DateTime.Today.Month);
        }

        private async void LoadInformation(int month)
        {
            await Task.Delay(250);
            var start = new DateTime(Year, month, 1);
            var end = start.AddMonths(1);
            if (CalenderHelper.RequireInfo(start))
            {
                IsBusy = true;
                Notices = await _noticeHandler.GetPostsDate(start, end);
                CalenderHelper.Insert(start, Notices);
                IsBusy = false;
            }
        }
    }
}
