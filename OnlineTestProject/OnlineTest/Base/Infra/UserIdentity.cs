using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Base.Infra
{
    public class UserIdentity : IUserIdentity
    {
        public bool IsLogin { get; set; }
        public int UserId { get; set; }
    }
}
