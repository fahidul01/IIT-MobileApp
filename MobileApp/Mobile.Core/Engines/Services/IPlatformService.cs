namespace Mobile.Core.Engines.Services
{
    public interface IPlatformService
    {
        void OpenToast(string text);
        void SubsubcribeTopics(params string[] topics);
        void UnsubscribeTopics(params string[] topics);
    }
}