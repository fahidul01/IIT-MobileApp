using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.Models.Partials
{
    public  class RoutineViewModel
    {
        public List<Activity> Activities { get;private set; }
        public List<Routine> Routines { get;private set; }

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
                if (item.IsSelected) Activities = item.Activities;
            }
        }

        public void Update(List<Lesson> lessons, List<Notice> notices)
        {
            var allActivity = new List<Activity>();
            lessons.ForEach(x => allActivity.Add(new Activity(x.Course?.CourseName, x.DayOfWeek, x.TimeOfDay)));
            notices.ForEach(x => allActivity.Add(new Activity(x.Title, x.EventDate.DayOfWeek, TimeSpan.FromHours(9))));

            foreach(var item in Routines)
            {
                var todayActivity = allActivity.Where(x => x.DayOfWeek == item.DayOfWeek)
                                               .OrderBy(x => x.TimeOfDay)
                                               .ToList();
                if (todayActivity.Count > 0) item.Activities = todayActivity;
                if (item.DayOfWeek == DateTime.Now.DayOfWeek)
                {
                    item.IsSelected = true;
                    Activities = item.Activities;
                }
                else item.IsSelected = false;
            }
        }
    }

    public class Routine
    {
        public DayOfWeek DayOfWeek { get; private set; }

        public Routine(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
            Day = DayOfWeek.ToString().Substring(0, 3);
            Activities = new List<Activity>();
        }

        public string Day { get; set; }
        public bool IsSelected { get; set; }
        public List<Activity> Activities { get; set; }
    }

    public class Activity
    {
        public Activity(string courseName, DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            Name = courseName;
            DayOfWeek = dayOfWeek;
            TimeOfDay = timeOfDay;
        }

        public string Name { get; set; }
        public DayOfWeek DayOfWeek { get; }
        public TimeSpan TimeOfDay { get; }
        public DateTime DateTime { get; set; }
    }
}
