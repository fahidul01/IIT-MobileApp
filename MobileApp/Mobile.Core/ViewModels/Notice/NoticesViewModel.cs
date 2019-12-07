using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mobile.Core.ViewModels
{
    public class NoticesViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        public List<Notice> UpcomingNotices { get; private set; }
        public ObservableCollection<Notice> Notices { get; private set; }

        public NoticesViewModel(INoticeHandler noticeHandler)
        {
            Notices = new ObservableCollection<Notice>();
            _noticeHandler = noticeHandler;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            Notices.Clear();
        }


    }
}
