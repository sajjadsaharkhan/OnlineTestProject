using OnlineTest.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTest.Services.Validators.Infra
{
    public interface IExamValidator
    {
        Task ValidateStartExam(Guid examUid, CancellationToken cancellationToken);
        Task ValidateEndExam(EndExamInputModel inputModel, CancellationToken cancellationToken);
    }
}
