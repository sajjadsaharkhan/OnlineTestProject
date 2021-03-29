using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineTest.Base
{
    public class MessagesDictionary
    {
        #region Errors
        public const string ValidationErrorMessage = "OneOrMoreValidationErrorsHaveOccurred";
        public const string InvalidStampCode = "YourSecurityStampCodeHasChanged";
        public const string EmptyStamCode = "YourSecurityStampCodeCantBeEmpty";
        public const string InvalidTokenClaims = "YourTokenClaimsIsInvalid";
        public const string LoginIsRequired = "YourTokenIsInvalidLoginAgain";
        public const string JsonFormatError = "TheEnteredJsonFormatIsInvalid";
        public const string NullOrEmptyErrorMessage = "ThisPropertyCantBeNullOrEmpty";


        //User
        public const string UserNotFoundErrorMessage = "UserNotFound";
        public const string WrongPasswordErrorMessage = "InvalidPassword";

        //Exam
        public const string ExamNotFoundErrorMessage = "ExamNotFound";
        public const string ExamStartTimeErrorMessage = "TheExamHasNotStartedYet";
        public const string ExamEndTimeErrorMessage = "TheTestHasExpired";
        public const string UserExamAccessErrorMessage = "YouDoNotHavePermissionToAccessThisExam";
        public const string RepeatExamErrorMessage = "YouHaveTakenThisTestOnce";
        public const string StartExamErrorMessage = "PleaseFirstStartTheExamAndThenSendAnswers";
        public const string QuestionNotFoundErrorMessage = "PleaseFillAllQuestionOfThisExam";
        public const string OptionNotFoundErrorMessage = "SelectedOptionNotFound";

        #endregion

        #region Messages
        public const string ServerError = "InternalError";
        public const string Success = "Success";
        #endregion
    }
}
