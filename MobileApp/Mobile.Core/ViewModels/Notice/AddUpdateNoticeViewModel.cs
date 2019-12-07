using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateNoticeViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        public Notice CurrentNotice { get; private set; }
        public AddUpdateNoticeViewModel(INoticeHandler noticeHandler)
        {
            _noticeHandler = noticeHandler;
        }



        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null & args.Length > 0 && args[0] is Notice notice)
            {
                CurrentNotice = notice;
            }
            else
            {
                CurrentNotice = new Notice();
            }
        }


        public ICommand SaveCommand => new RelayCommand(SaveAction);
        public ICommand EditorCommand => new RelayCommand(EditorAction);

        private void EditorAction()
        {
            var dt = new RelayCommand<string>(x => CurrentNotice.Message = x);
            _nav.NavigateToModal<EditorViewModel>(dt);
        }

        private async void SaveAction()
        {

            if (CurrentNotice.Id == 0)
            {
                var res = await _noticeHandler.AddPost(CurrentNotice);
                ShowResponse(res);
            }
            else
            {
                var res = await _noticeHandler.UpdatePost(CurrentNotice);
                ShowResponse(res);
            }
        }
    }
}
