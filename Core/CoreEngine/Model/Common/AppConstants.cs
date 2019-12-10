namespace CoreEngine.Model.Common
{
    public class AppConstants
    {
        public const string Name = "IITMobile";
        public const string Student = "Student";
        public const string Admin = "Admin";
#if DEBUG
        public static string BaseUrl = "http://192.168.1.106:5001/api/";
#else
        public static string BaseUrl = "https://mit.techapp.ml/api/";
#endif
        public static string DataPath;
    }
}
