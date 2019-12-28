using CoreEngine.Model.DBModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.Engines.Dependency
{
    public interface ICalenderHelper
    {
        Dictionary<DateTime, ICollection> EventDatas { get; }
        void Insert(DateTime month, List<Notice> notices);
        void Clear();
        bool RequireInfo(DateTime start);
    }
}
