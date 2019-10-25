using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoreEngine.Engine
{
    public class LogEngine
    {
        public static event EventHandler<string> ErrorOccured;

        public static bool IsDetailed = false;
        public static void Error(Exception ex, [CallerMemberName]string name = "")
        {
            string msg;
            if (IsDetailed)
            {
                msg = string.Format("{0} [{1}] => {2}", DateTime.Now.ToShortTimeString(), name, ex.ToString());
            }
            else
            {
                msg = ex.Message;
            }
            ErrorOccured?.Invoke(null, msg);
        }
    }
}
