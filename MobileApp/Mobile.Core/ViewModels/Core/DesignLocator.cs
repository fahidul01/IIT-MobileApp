using Mobile.Core.Engines.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.ViewModels
{
    public static class DesignLocator
    {
        public static LoginViewModel LoginViewModel => Locator.GetInstance<LoginViewModel>();
        public static ForgetPassViewModel ForgetPassViewModel => Locator.GetInstance<ForgetPassViewModel>();
        public static AddUpdateCourseViewModel AddUpdateCourseViewModel => Locator.GetInstance<AddUpdateCourseViewModel>();
        public static AddUpdateLessonViewModel AddUpdateLessonViewModel => Locator.GetInstance<AddUpdateLessonViewModel>();
        public static CourseDetailViewModel CourseDetailViewModel => Locator.GetInstance<CourseDetailViewModel>();
        public static CoursesViewModel CoursesViewModel => Locator.GetInstance<CoursesViewModel>();
        public static HomeViewModel HomeViewModel => Locator.GetInstance<HomeViewModel>();
        public static AddUpdateNoticeViewModel AddUpdateNoticeViewModel => Locator.GetInstance<AddUpdateNoticeViewModel>();
        public static NoticeDetailViewModel NoticeDetailViewModel => Locator.GetInstance<NoticeDetailViewModel>();
        public static NoticesViewModel NoticesViewModel => Locator.GetInstance<NoticesViewModel>();
        public static ProfileDetailViewModel ProfileDetailViewModel => Locator.GetInstance<ProfileDetailViewModel>();
        public static ProfilesViewModel ProfilesViewModel => Locator.GetInstance<ProfilesViewModel>();
        public static SplashViewModel SplashViewModel => Locator.GetInstance<SplashViewModel>();

    }
}
