using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTest.Base;
using OnlineTest.Base.Exceptions;
using OnlineTest.Base.Extensions;
using OnlineTest.Bootstrapper.Filters;
using OnlineTest.Bootstrapper.Jwt.Infra;
using OnlineTest.InputModels;
using Project.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiResultFilterAttribute]
    public class UserController : Controller
    {
        private readonly OnlineTestContext context;
        private readonly IJwtHandel jwtHandel;

        public UserController(OnlineTestContext _context, IJwtHandel jwtHandel)
        {
            context = _context;
            this.jwtHandel = jwtHandel;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginInputModel inputModel, CancellationToken cancellationToken)
        {
            //Validations: 
            //      Check that the username and password are empty?
            if (inputModel.Username.Trim().IsNullOrEmpty())
                throw new AppException("Username", MessagesDictionary.NullOrEmptyErrorMessage);
            if (inputModel.Password.Trim().IsNullOrEmpty())
                throw new AppException("Password", MessagesDictionary.NullOrEmptyErrorMessage);

            var user = await context.Users.Where(x => x.Username == inputModel.Username).FirstOrDefaultAsync(cancellationToken);

            //      Check user is in database?
            if (user == null)
                throw new AppException(MessagesDictionary.UserNotFoundErrorMessage);
            //      Check entered password is equal to main user passwor or no!
            if (user.Password != inputModel.Password)
                throw new AppException(MessagesDictionary.WrongPasswordErrorMessage);

            //create new stamp code to logout other user sessions!
            user.Stamp = Guid.NewGuid();

            //send update user transaction to database
            await context.SaveChangesAsync(cancellationToken);

            var tokenModel = jwtHandel.GenerateToken(user);

            return ApiResult<TokenModel>.Ok(tokenModel);
        }
    }
}
