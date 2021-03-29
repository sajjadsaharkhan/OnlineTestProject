using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class ExamUserAnswer
    {
        public int Id { get; set; }
        public int ExamUserId { get; set; }
        public int ExamQuestionId { get; set; }
        public int? SelectedOptionId { get; set; }
        public string BlankQuestionAnswer { get; set; }
        public string DescriptiveAnswer { get; set; }

        public virtual ExamQuestion ExamQuestion { get; set; }
        public virtual ExamUser ExamUser { get; set; }
        public virtual QuestionOption SelectedOption { get; set; }
    }
}
