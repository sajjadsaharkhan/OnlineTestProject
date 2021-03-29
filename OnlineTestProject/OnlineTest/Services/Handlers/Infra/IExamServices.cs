using OnlineTest.InputModels;
using OnlineTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTest.Services.Handlers.Infra
{
    public interface IExamServices
    {
        Task<ExamViewModel> HandleStartExam(Guid examUid, CancellationToken cancellationToken);
        Task<List<QuestionWithAnswerViewModel>> HandleEndExam(EndExamInputModel inputModel, CancellationToken cancellationToken);
    }
}