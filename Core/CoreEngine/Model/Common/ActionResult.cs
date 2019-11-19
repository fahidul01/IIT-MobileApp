namespace CoreEngine.Model.Common
{
    public class ActionResponse
    {
        public bool Actionstatus { get; set; }
        public string Message { get; set; }

        public ActionResponse(bool state, string message)
        {
            Actionstatus = state;
            Message = message;
        }
    }
}
