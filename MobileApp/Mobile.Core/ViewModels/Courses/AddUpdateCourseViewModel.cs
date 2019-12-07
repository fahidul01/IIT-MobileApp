﻿using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateCourseViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;
        private readonly IPreferenceEngine _filePicker;

        public Course CurrentCourse { get; private set; }
        public ObservableCollection<Lesson> Lessons { get; private set; }
        public ObservableCollection<DBFile> DBFiles { get; private set; }
        public List<Semester> Semesters { get; private set; }
        public Semester CurrentSemester { get; set; }

        public AddUpdateCourseViewModel(ICourseHandler courseHandler, IPreferenceEngine preferenceEngine)
        {
            _courseHandler = courseHandler;
            _filePicker = preferenceEngine;
            Lessons = new ObservableCollection<Lesson>();
            DBFiles = new ObservableCollection<DBFile>();
        }
        public override async void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args[0] is Course course)
            {
                CurrentCourse = course;
                await LoadCourseDataAsync(course);
            }
            else
            {
                CurrentCourse = new Course();
            }
        }

        private async Task LoadCourseDataAsync(Course course)
        {
            var fullCourse = await _courseHandler.GetCourse(course.Id);
            if (fullCourse != null)
            {
                DBFiles.Clear();
                Lessons.Clear();
                foreach (var item in fullCourse.CourseMaterials)
                {
                    DBFiles.Add(item);
                }
                foreach (var item in fullCourse.Lessons)
                {
                    Lessons.Add(item);
                }
            }
            Semesters = await _courseHandler.GetCurrentSemester();
            CurrentSemester = Semesters.FirstOrDefault(x => x.StartsOn >= DateTime.Now);
        }

        public ICommand SaveCommand => new RelayCommand(SaveAction);
        public ICommand AddMaterialCommand => new RelayCommand(AddMaterialAction);
        public ICommand DeleteMaterialComand => new RelayCommand<DBFile>(DeleteMaterialAction);
        public ICommand AddLessonCommand => new RelayCommand(AddLessonAction);
        public ICommand EditLessonCommand => new RelayCommand<Lesson>(EditLessonAction);
        public ICommand DeleteLessonCommand => new RelayCommand<Lesson>(DeleteLessonAction);

        private async void DeleteLessonAction(Lesson obj)
        {
            var confirm = await _dialog.ShowConfirmation("Confirm", "Are you sure to delete this lesson?");
            if (confirm)
            {
                var response = await _courseHandler.DeleteLesson(obj.Id);
                ShowResponse(response);
            }
        }



        private void EditLessonAction(Lesson obj)
        {
            _nav.NavigateTo<AddUpdateLessonViewModel>(CurrentCourse, obj);
        }

        private void AddLessonAction()
        {
            _nav.NavigateTo<AddUpdateLessonViewModel>(CurrentCourse, new Lesson());
        }

        private async void DeleteMaterialAction(DBFile obj)
        {
            var confirm = await _dialog.ShowConfirmation("Confirm", "Are you sure you want to delete this material");
            if (confirm)
            {
                IsBusy = true;
                var res = await _courseHandler.DeleteCouseMaterial(obj);
                if (res != null)
                {
                    _dialog.ShowToastMessage(res.Message);
                }

                if (res.Actionstatus)
                {
                    DBFiles.Remove(obj);
                }

                IsBusy = false;
            }
        }

        private async void AddMaterialAction()
        {
            IsBusy = true;
            var file = await _filePicker.PickFile();
            if (file != null)
            {

                if (CurrentCourse.Id != 0)
                {
                    var res = await _courseHandler.AddMaterial(CurrentCourse.Id, new List<DBFile> { file });
                    if (res != null)
                    {
                        _dialog.ShowToastMessage(res.Message);
                        if (res.Actionstatus)
                        {
                            DBFiles.Add(file);
                        }
                    }
                }
                else
                {
                    DBFiles.Add(file);
                }
            }
            IsBusy = false;
        }

        private async void SaveAction()
        {
            if (string.IsNullOrEmpty(CurrentCourse.CourseName))
            {
                _dialog.ShowToastMessage("Invalid Name");
            }
            else if (string.IsNullOrEmpty(CurrentCourse.CourseId))
            {
                _dialog.ShowToastMessage("Incalid Course ID");
            }
            else if (CurrentCourse.CourseCredit <= 0)
            {
                _dialog.ShowToastMessage("Credit hour should be greater than 0");
            }
            else
            {
                if (CurrentCourse.Id == 0)
                {
                    var res = await _courseHandler.CreateCourse(CurrentSemester.Id, CurrentCourse, DBFiles.ToList());
                    if (res.Actionstatus)
                    {
                        _nav.GoBack();
                    }

                    _dialog.ShowToastMessage("Created Course Successfully");
                }
                else
                {
                    var res = await _courseHandler.UpdateCourse(CurrentCourse);
                    if (res.Actionstatus)
                    {
                        _nav.GoBack();
                    }

                    _dialog.ShowToastMessage("Updated Course Successfully");
                }
            }
        }
    }
}
