using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateNoticeViewModel : BaseViewModel
    {
        private readonly INoticeHandler _noticeHandler;
        public Notice CurrentNotice { get; private set; }
        public IEnumerable<PostType> PostTypes { get; set; }
        public PostType CurrentPost { get; set; }
        public ObservableCollection<DBFile> DBFiles { get; set; }
        public AddUpdateNoticeViewModel(INoticeHandler noticeHandler)
        {
            _noticeHandler = noticeHandler;
            PostTypes = Enum.GetValues(typeof(PostType)).Cast<PostType>();
            DBFiles = new ObservableCollection<DBFile>();
        }



        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args.Length > 0 && args[0] is Notice notice)
            {
                CurrentNotice = notice;
            }
            else if (args.Length > 0 && args[0] is PostType postType)
            {
                CurrentNotice = new Notice
                {
                    PostType = postType
                };
            }
            else
            {
                CurrentNotice = new Notice();
            }
            CurrentPost = PostTypes.FirstOrDefault(x => x == CurrentNotice.PostType);
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
            if (string.IsNullOrWhiteSpace(CurrentNotice.Title))
            {
                _dialog.ShowMessage("Error", "Invalid Title");
            }
            else if (string.IsNullOrWhiteSpace(CurrentNotice.Message))
            {
                _dialog.ShowMessage("Error", "Invalid Message");
            }
            else
            {
                CurrentNotice.PostType = CurrentPost;
                if (CurrentNotice.Id == 0)
                {
                    var res = await _noticeHandler.AddPost(CurrentNotice, DBFiles.ToList());
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
}
