using Microsoft.EntityFrameworkCore;
using OnlineTest.Base;
using OnlineTest.Base.Exceptions;
using OnlineTest.Base.Infra;
using OnlineTest.InputModels;
using OnlineTest.Services.Validators.Infra;
using Project.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineTest.Services.Validators
{
    public class ExamValidator : IExamValidator
    {
        private readonly OnlineTestContext context;
        private readonly IUserIdentity userIdentity;

        public ExamValidator(OnlineTestContext context, IUserIdentity userIdentity)
        {
            this.context = context;
            this.userIdentity = userIdentity;
        }

        public async Task ValidateStartExam(Guid examUid, CancellationToken cancellationToken)
        {
            //Check exam uid is a Empty Guid?
            if (examUid == Guid.Empty)
                throw new AppException("ExamUid", MessagesDictionary.NullOrEmptyErrorMessage);

            //fetch exam from database and check it exist or no!
            var exam = await context.Exams.AsNoTracking()
                                .Where(x => x.Uid == examUid)
                                .Select(x => new
                                {
                                    x.Id,
                                    x.StartDate,
                                    x.EndDate
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            if (exam == null)
                throw new AppException(MessagesDictionary.ExamNotFoundErrorMessage);

            //Check exam start and end time!
            //  if now is grather than start date means exam not started yet!
            //  if now is less than end date means exam has expired!
            if (exam.StartDate > DateTime.Now)
                throw new AppException(MessagesDictionary.ExamStartTimeErrorMessage);
            if (exam.EndDate < DateTime.Now)
                throw new AppException(MessagesDictionary.ExamEndTimeErrorMessage);

            //check that current user has access to this exam or no!
            var userExam = await context.ExamUsers.AsNoTracking()
                                                .AnyAsync(x => x.UserId == userIdentity.UserId && x.ExamId == exam.Id, cancellationToken);
            if (!userExam)
                throw new AppException(MessagesDictionary.UserExamAccessErrorMessage);
        }


        public async Task ValidateEndExam(EndExamInputModel inputModel, CancellationToken cancellationToken)
        {
            //Check exam uid is a Empty Guid?
            if (inputModel.ExamUid == Guid.Empty)
                throw new AppException("ExamUid", MessagesDictionary.NullOrEmptyErrorMessage);

            //fetch exam from database and check it exist or no!
            var exam = await context.Exams.AsNoTracking()
                                .Where(x => x.Uid == inputModel.ExamUid)
                                .Select(x => new
                                {
                                    x.Id,
                                    x.StartDate,
                                    x.EndDate
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            if (exam == null)
                throw new AppException(MessagesDictionary.ExamNotFoundErrorMessage);

            //Check exam start and end time!
            //  if now is grather than start date means exam not started yet!
            //  if now is less than end date means exam has expired!
            if (exam.StartDate > DateTime.Now)
                throw new AppException(MessagesDictionary.ExamStartTimeErrorMessage);
            if (exam.EndDate < DateTime.Now)
                throw new AppException(MessagesDictionary.ExamEndTimeErrorMessage);

            //check that current user has access to this exam or no!
            var userExam = await context.ExamUsers.AsNoTracking()
                                                .Where(x => x.UserId == userIdentity.UserId && x.ExamId == exam.Id)
                                                .FirstOrDefaultAsync(cancellationToken);
            if (userExam == null)
                throw new AppException(MessagesDictionary.UserExamAccessErrorMessage);

            //Check that the user has already passed this exam?
            if (userExam.IsFinilized || userExam.IsEnded)
                throw new AppException(MessagesDictionary.RepeatExamErrorMessage);

            //fetch exam questions and user submitted questions for comparison
            var questionUids = inputModel.Answers.Select(x => x.QuestionUid).ToList();
            var questionIds = await context.ExamQuestions.AsNoTracking()
                                                     .Where(x => x.ExamId == exam.Id)
                                                     .Select(x => x.QuestionId)
                                                     .ToListAsync(cancellationToken);
            //if exam questions count not equal to user submitted questions
            //that means user not send all exam questions or sened question from another exam
            if (questionIds.Count != questionUids.Count)
                throw new AppException(MessagesDictionary.QuestionNotFoundErrorMessage);

            //Check Question Options is correct! selected option of question are exist in database or no?

            //The first way:

            //var options = await context.QuestionOptions.AsNoTracking()
            //                                        .Where(x => questionIds.Contains(x.QuestionId))
            //                                        .Include(x => x.Question)
            //                                        .Select(x => new
            //                                        {
            //                                            OptionUid = x.Uid,
            //                                            QuestionUid = x.Question.Uid,
            //                                        }).ToListAsync(cancellationToken);

            //var optionGroup = options.GroupBy(x => x.QuestionUid).ToList();
            //var qq = inputModel.Answers
            //    .Select(a => optionGroup.Any(og => og.Key == a.QuestionUid && og.Any(o => o.OptionUid == a.SelectedOptionUid)))
            //    .Distinct()
            //    .ToList();


            //a better way:
            var optionUids = inputModel.Answers.Select(x => x.SelectedOptionUid).ToList();
            var options = await context.QuestionOptions.AsNoTracking()
                                                    .Where(x => optionUids.Contains(x.Uid))
                                                    .Select(x => new
                                                    {
                                                        x.Uid,
                                                        QuestionUid = x.Question.Uid,
                                                    }).ToListAsync(cancellationToken);
            //if this condition is not correct thats mean The user has selected an option that does not exist in the database
            if (options.Count != optionUids.Count)
                throw new AppException(MessagesDictionary.OptionNotFoundErrorMessage);

            //Check if the selected option is relevant to this question
            var optionFlags = inputModel.Answers
                .Select(x => options.Any(w => w.Uid == x.SelectedOptionUid && w.QuestionUid == x.QuestionUid))
                .Distinct()
                .ToList();

            if (optionFlags.Contains(false))
                throw new AppException(MessagesDictionary.OptionNotFoundErrorMessage);
        }
    }
}
