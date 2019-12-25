using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;

namespace Student.Infrastructure.AppServices
{
    class EmailMessageCreator
    {
        internal static string CreatePasswordRecovery()
        {
            return string.Format("We have recently received a password " +
                "recovery request from the admin panel. " +
                "\nIf this was not intentional, please contact admin " +
                "as soon as possible");
        }

        internal static string CreateInvitation(string mobile)
        {
            return string.Format("You have been successfully registered for IIT, DU." +
                "Please download the application from  " + AppConstants.BaseUrl + "/download/IIT.APK" +
                 "And then follow the registration process" +
                 "Your mobile no used for registration: <b>{0}</b>", mobile);
        }
    }
}
