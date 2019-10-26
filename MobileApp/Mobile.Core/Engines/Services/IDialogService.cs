using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface IDialogService
    {
        void ShowMessage(string title, string message);
        Task<bool> ShowConfirmation(string title, string meaage);
        Task<string> ShowAction(string title, string cancel, params object[] actions);
        void ShowAction(string title, string cancel, Dictionary<string, Action> actions);
        void ShowToastMessage(string message);
    }
}
