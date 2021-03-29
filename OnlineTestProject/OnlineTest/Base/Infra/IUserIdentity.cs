using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Base.Infra
{
    public interface IUserIdentity
    {
        bool IsLogin { get; set; }
        int UserId { get; set; }
    }
}
