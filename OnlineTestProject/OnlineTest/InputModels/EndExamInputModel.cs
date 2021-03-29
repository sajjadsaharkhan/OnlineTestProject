using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.InputModels
{
    public class EndExamInputModel
    {
        public Guid ExamUid { get; set; }
        public List<QuestionInputModel> Answers { get; set; }
    }

    public class QuestionInputModel
    {
        public Guid QuestionUid { get; set; }
        public Guid? SelectedOptionUid { get; set; }
    }
}
