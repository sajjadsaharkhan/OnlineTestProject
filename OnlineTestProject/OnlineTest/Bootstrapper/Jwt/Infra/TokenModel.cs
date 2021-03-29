using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Bootstrapper.Jwt.Infra
{
    public class TokenModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public TokenModel() { }
        public TokenModel(string name, string lastname, string token, DateTime expires, string username)
        {
            Firstname = name;
            Lastname = lastname;
            Username = username;
            Token = token;
            Expires = expires;
        }
    }
}
