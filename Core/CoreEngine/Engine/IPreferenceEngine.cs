using CoreEngine.Model.DBModel;
using System.Threading.Tasks;

namespace CoreEngine.Engine
{
    public interface IPreferenceEngine
    {
        string GetSetting(string key, string value);
        void SetSetting(string key, string value);
    }
}
