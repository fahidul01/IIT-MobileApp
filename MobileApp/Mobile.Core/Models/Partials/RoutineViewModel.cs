using CoreEngine.Model.DBModel;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.Models.Partials
{
    public class RoutineViewModel:NotifyModel
    {
        public List<Activity> Activities { get; private set; }
        public List<Routine> Routines { get; private set; }

        public RoutineViewModel()
        {
            Routines = new List<Routine>()
            {
                new Routine(DayOfWeek.Saturday),
                new Routine(DayOfWeek.Sunday),
                new Routine(DayOfWeek.Monday),
                new Routine(DayOfWeek.Tuesday),
                new Routine(DayOfWeek.Wednesday),
                new Routine(DayOfWeek.Thursday),
                new Routine(DayOfWeek.Friday)
            };
        }

        public void SelectRoutine(Routine routine)
        {
            foreach (var item in Routines)
            {
                item.IsSelected = item == routine;
                if (item.IsSelected)
                {
                    Activities = item.Activities;
                }
            }
        }

        public void Update(List<Lesson> lessons, List<Notice> notices)
        {
            var allActivity = new List<Activity>();
            lessons.ForEach(x => 
            allActivity.Add(new Activity(x.Course?.CourseName,"Lesson", x.DayOfWeek, x.TimeOfLesson)));
            notices.ForEach(x => 
            allActivity.Add(new Activity(x.Title, x.PostType.ToString(), x.EventDate.DayOfWeek, x.TimeOfEvent)));

            foreach (var item in Routines)
            {
                var todayActivity = allActivity.Where(x => x.DayOfWeek == item.DayOfWeek)
                                               .OrderBy(x => x.TimeOfDay)
                                               .ToList();
                if (todayActivity.Count > 0)
                {
                    item.Activities = todayActivity;
                }
            }
        }
    }

    public class Routine : NotifyModel
    {
        public DayOfWeek DayOfWeek { get; private set; }

        public Routine(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
            Day = DayOfWeek.ToString().Substring(0, 3);
            Activities = new List<Activity>();
            IsSelected = DayOfWeek == DateTime.Now.DayOfWeek;
        }

        public string Day { get; set; }
        public bool IsSelected { get; set; }
        public List<Activity> Activities { get; set; }
    }

    public class Activity
    {
        public Activity(string name, 
            string description, DayOfWeek dayOfWeek,
            string timeOfDay)
        {
            Name = name;
            DayOfWeek = dayOfWeek;
            TimeOfDay = timeOfDay;
            Description = description;
        }

        public string Name { get; set; }
        public DayOfWeek DayOfWeek { get; }
        public string TimeOfDay { get; }
        public string Description { get; }
        public DateTime DateTime { get; set; }
    }

    public enum ActivityType
    {
        Lesson,
        Notice
    }
}
