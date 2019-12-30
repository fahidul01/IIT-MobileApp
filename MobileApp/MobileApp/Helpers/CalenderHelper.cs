using CoreEngine.Model.DBModel;
using Mobile.Core.Engines.Dependency;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Plugin.Calendar.Models;

namespace MobileApp.Helpers
{
    public class CalenderHelper : ICalenderHelper
    {
        public Dictionary<DateTime, ICollection> EventDatas => GetEventData();

        private readonly EventCollection EventCollection;
        private readonly List<DateTime> CollectedMonth;
        public CalenderHelper()
        {
            EventCollection = new EventCollection();
            CollectedMonth = new List<DateTime>();
        }
        private Dictionary<DateTime, ICollection> GetEventData()
        {
            return EventCollection;
        }

        public void Insert(DateTime dateTime, List<Notice> notices)
        {
            if (CollectedMonth.Contains(dateTime) == false)
                CollectedMonth.Add(dateTime);
            foreach (var group in notices.GroupBy(x => x.EventDate))
            {
                if (!EventCollection.ContainsKey(group.Key.Date))
                    EventCollection.Add(group.Key.Date, group.ToList());
            }
        }

        public void Clear()
        {
            EventCollection.Clear();
            CollectedMonth.Clear();
        }

        public bool RequireInfo(DateTime start)
        {
            return !CollectedMonth.Contains(start);
        }
    }
}
