using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;

namespace Mobile.Core.ViewModels
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;
        public List<SemesterData> SemesterDatas { get; set; }
        public GradesViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }
        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            RefreshAction();
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            SemesterDatas = await _courseHandler.GetResult();
            IsRefreshisng = false;
        }
    }
}
