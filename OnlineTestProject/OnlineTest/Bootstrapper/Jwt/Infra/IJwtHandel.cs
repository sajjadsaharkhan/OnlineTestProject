using Project.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Bootstrapper.Jwt.Infra
{
    public interface IJwtHandel
    {
        TokenModel GenerateToken(User user);
    }
}
