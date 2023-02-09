﻿using System.Net;

namespace Server.Controllers.Users
{
    public class LoginResponseModel
    {
        public HttpStatusCode code { get; set; }
        public string content { get; set; }

        public LoginResponseModel(HttpStatusCode code, string content)
        {
            this.code = code;
            this.content = content;
        }
    }
}
