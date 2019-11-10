using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEngine.Model.Common
{
    public abstract class BaseModel
    {
        public DateTime CurrentTime => DateTime.UtcNow.AddHours(6);
    }
}
