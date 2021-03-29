using Microsoft.AspNetCore.Mvc;
using OnlineTest.Base;
using OnlineTest.Bootstrapper.Filters;
using OnlineTest.InputModels;
using OnlineTest.Services.Handlers.Infra;
using OnlineTest.Services.Validators.Infra;
using OnlineTest.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiResultFilterAttribute]
    public class ExamController : ControllerBase
    {
        private readonly IExamValidator examValidator;
        private readonly IExamServices examServices;

        public ExamController(IExamValidator examValidator, IExamServices examServices)
        {
            this.examValidator = examValidator;
            this.examServices = examServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> StartExam([FromBody] StartExamInputModel inputModel, CancellationToken cancellationToken)
        {
            await examValidator.ValidateStartExam(inputModel.ExamUid, cancellationToken);
            var result = await examServices.HandleStartExam(inputModel.ExamUid, cancellationToken);
            return ApiResult<ExamViewModel>.Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> EndExam([FromBody] EndExamInputModel inputModel, CancellationToken cancellationToken)
        {
            await examValidator.ValidateEndExam(inputModel, cancellationToken);
            var result = await examServices.HandleEndExam(inputModel, cancellationToken);
            return ApiResult<List<QuestionWithAnswerViewModel>>.Ok(result);
        }
    }
}
