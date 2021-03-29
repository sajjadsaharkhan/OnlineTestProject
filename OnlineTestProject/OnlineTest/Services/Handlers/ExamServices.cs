using Microsoft.EntityFrameworkCore;
using OnlineTest.Base;
using OnlineTest.Base.Infra;
using OnlineTest.InputModels;
using OnlineTest.Services.Handlers.Infra;
using OnlineTest.ViewModels;
using Project.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTest.Services.Handlers
{
    public class ExamServices : IExamServices
    {
        private readonly OnlineTestContext context;
        private readonly IUserIdentity userIdentity;

        public ExamServices(OnlineTestContext context, IUserIdentity userIdentity)
        {
            this.context = context;
            this.userIdentity = userIdentity;
        }
        public async Task<ExamViewModel> HandleStartExam(Guid examUid, CancellationToken cancellationToken)
        {
            //Fetch Exam Information From Database :

            var exam = await context.Exams.AsNoTracking()
                                        .Where(x => x.Uid == examUid)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Uid,
                                            x.Title
                                        })
                                        .FirstOrDefaultAsync(cancellationToken);


            // the first way:

            //var subSections = await context.ExamSubSections.AsNoTracking()
            //                                            .Where(x => x.ExamId == exam.Id)
            //                                            .Include(x => x.ExamQuestions).ThenInclude(x => x.Question).ThenInclude(x => x.QuestionOptions)
            //                                            .Select(x => new ExamSubSectionViewModel
            //                                            {
            //                                                Title = x.Title,
            //                                                Description = x.Description,
            //                                                Questions = x.ExamQuestions.Select(q => new QuestionViewModel
            //                                                {
            //                                                    Uid = q.Question.Uid,
            //                                                    Question = q.Question.Question1,
            //                                                    Type = (QuestionTypes)q.Question.Type,
            //                                                    Title = q.Question.Title,
            //                                                    Options = q.Question.QuestionOptions.Select(o => new QuestionOptionsViewModel
            //                                                    {
            //                                                        Uid = o.Uid,
            //                                                        Title = o.Title
            //                                                    }).ToList()
            //                                                }).ToList()
            //                                            }).ToListAsync(cancellationToken);


            // a better way:
            //      *This way is about 1.5 times faster but the first way is so cleaner than this way

            var subSections = await context.ExamSubSections.AsNoTracking()
                                                        .Where(x => x.ExamId == exam.Id)
                                                        .Select(x => new
                                                        {
                                                            x.Id,
                                                            x.Title,
                                                            x.Description,
                                                        })
                                                        .ToListAsync(cancellationToken);

            var questions = await context.ExamQuestions.AsNoTracking()
                                                    .Include(x => x.Question)
                                                    .Where(x => x.ExamId == exam.Id)
                                                    .Select(x => new { x.SubSectionId, x.Question })
                                                    .ToListAsync(cancellationToken);

            var questionIds = questions.Select(x => x.Question.Id).ToList();
            var options = await context.QuestionOptions.AsNoTracking()
                                                    .Where(x => questionIds.Contains(x.QuestionId))
                                                    .Select(x => new
                                                    {
                                                        x.Uid,
                                                        x.Title,
                                                        x.QuestionId
                                                    }).ToListAsync(cancellationToken);

            var subSectionsViewModel = new List<ExamSubSectionViewModel>();
            foreach (var subsection in subSections)
            {
                var questionsViewModel = questions
                            .Where(x => x.SubSectionId == subsection.Id)
                            .Select(x => new QuestionViewModel
                            {
                                Uid = x.Question.Uid,
                                Title = x.Question.Title,
                                Type = (QuestionTypes)x.Question.Type,
                                Question = x.Question.Question1,
                                Options = options
                                            .Where(o => o.QuestionId == x.Question.Id)
                                            .Select(o => new QuestionOptionsViewModel
                                            {
                                                Uid = o.Uid,
                                                Title = o.Title,
                                            }).ToList()
                            }).ToList();

                subSectionsViewModel.Add(new ExamSubSectionViewModel
                {
                    Title = subsection.Title,
                    Description = subsection.Description,
                    Questions = questionsViewModel
                });
            }


            var examViewModel = new ExamViewModel()
            {
                Uid = exam.Uid,
                Title = exam.Title,
                SubSections = subSectionsViewModel
            };

            //set user exam properties:
            var examUser = await context.ExamUsers.Where(x => x.ExamId == exam.Id && x.UserId == userIdentity.UserId)
                                                    .FirstOrDefaultAsync(cancellationToken);

            examUser.IsParticipated = true;
            examUser.ExamStartDate = DateTime.Now;

            await context.SaveChangesAsync();

            return examViewModel;
        }

        public async Task<List<QuestionWithAnswerViewModel>> HandleEndExam(EndExamInputModel inputModel, CancellationToken cancellationToken)
        {
            var exam = await context.Exams.AsNoTracking()
                                        .Where(x => x.Uid == inputModel.ExamUid)
                                        .FirstOrDefaultAsync(cancellationToken);


            var examUser = await context.ExamUsers.Where(x => x.UserId == userIdentity.UserId && x.ExamId == exam.Id)
                                                    .FirstOrDefaultAsync(cancellationToken);

            var selectedOptionUids = inputModel.Answers.Select(x => x.SelectedOptionUid).ToList();
            var questionsUids = inputModel.Answers.Select(x => x.QuestionUid).ToList();

            var options = await context.QuestionOptions.AsNoTracking()
                                                    .Where(x => selectedOptionUids.Contains(x.Uid))
                                                    .Select(x => new
                                                    {
                                                        x.Id,
                                                        x.QuestionId
                                                    }).ToListAsync(cancellationToken);
            var questions = await context.ExamQuestions.AsNoTracking()
                                                .Include(x => x.Question)
                                                .Where(x => x.ExamId == exam.Id && questionsUids.Contains(x.Question.Uid))
                                                .Select(x => new
                                                {
                                                    ExamQuestionId = x.Id,
                                                    QuestionId = x.QuestionId,
                                                    QuestionVM = new QuestionWithAnswerViewModel
                                                    {
                                                        Uid = x.Question.Uid,
                                                        Type = (QuestionTypes)x.Question.Type,
                                                        Title = x.Question.Title,
                                                        Question = x.Question.Question1,
                                                        DescriptiveAnswer = x.Question.DescriptiveAnswer,
                                                    }
                                                })
                                                .ToListAsync(cancellationToken);

            var examUserAnswers = questions.Select(x => new ExamUserAnswer
            {
                SelectedOptionId = options.Where(o => o.QuestionId == x.QuestionId).Select(x => x.Id).FirstOrDefault(),
                ExamUserId = examUser.Id,
                ExamQuestionId = x.ExamQuestionId,
            }).ToList();

            context.ExamUserAnswers.AddRange(examUserAnswers);

            examUser.ExamEndDate = DateTime.Now;
            examUser.IsEnded = true;

            await context.SaveChangesAsync();

            return questions.Select(x => x.QuestionVM).ToList();
        }
    }
}
