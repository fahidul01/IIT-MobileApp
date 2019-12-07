namespace Web.Infrastructure.AppServices
{
    class EmailMessageCreator
    {
        internal string CreatePasswordRecovery(string password)
        {
            return string.Format("We have recently received a password recovery request from the admin panel. " +
                "We have created a new password.<strong>{0}</strong>", password);
        }
    }
}
