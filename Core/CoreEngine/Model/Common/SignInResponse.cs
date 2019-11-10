using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEngine.Model.Common
{
    public class SignInResponse
    {
        public SignInResponse(bool status)
        {
            Success = status;
        }
        public SignInResponse(bool status, string token)
        {
            Success = status;
            Token = token;
        }
        public SignInResponse()
        {
        }
        public bool Success { get; set; }
        public string Token { get; set; }
    }
}
