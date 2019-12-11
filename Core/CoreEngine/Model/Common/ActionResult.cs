namespace CoreEngine.Model.Common
{
    public class ActionResponse
    {
        public bool Actionstatus { get; set; }
        public string Message { get; set; }
        public string ResponseCode { get; set; }
        public ActionResponse(bool state, string message)
        {
            Actionstatus = state;
            Message = message;
        }

        public ActionResponse(bool res)
        {
            Actionstatus = res;
            Message = res ? "Success" : "Failed to complete the operation";
        }

        public ActionResponse()
        {

        }
    }
}
