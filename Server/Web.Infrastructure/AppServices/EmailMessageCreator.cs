using CoreEngine.Model.Common;

namespace Student.Infrastructure.AppServices
{
    class EmailMessageCreator
    {
        internal static string CreatePasswordRecovery(string password)
        {
            return string.Format("We have recently received a password recovery request from the admin panel. " +
                "We have created a new password.<strong>{0}</strong>", password);
        }

        internal static string CreateInvitation(string password)
        {
            return string.Format("You have been successfully registered for IIT, DU." +
                "Please download the application from  " + AppConstants.BaseUrl + "/download/IIT.APK" +
                 "We have created a new password.<strong>{0}</strong>", password);
        }
    }
}
