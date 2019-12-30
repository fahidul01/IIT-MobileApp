using System;
using System.Collections;
using System.Collections.Generic;

namespace Mobile.Core.Models.Core
{
    public class ActivityCollection
    {
        readonly Dictionary<DateTime, ICollection> Activities;
        public ActivityCollection()
        {
            Activities = new Dictionary<DateTime, ICollection>();
        }

        public void AddData(DateTime dateTime, ICollection collection)
        {
            if (!Activities.ContainsKey(dateTime.Date))
            {
                Activities.Add(dateTime.Date, collection);
                DataChanged?.Invoke(this, null);
            }
        }

        public void Clear()
        {
            Activities.Clear();
        }

        public ICollection GetCollection(DateTime dateTime)
        {
            if (Activities.TryGetValue(dateTime.Date, out ICollection _info))
            {
                return _info;
            }
            else
            {
                return null;
            }
        }

        public event EventHandler DataChanged;
    }
}
