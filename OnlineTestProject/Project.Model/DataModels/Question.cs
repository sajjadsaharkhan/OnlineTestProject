using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class Question
    {
        public Question()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            QuestionOptions = new HashSet<QuestionOption>();
            SubjectQuestions = new HashSet<SubjectQuestion>();
        }

        public int Id { get; set; }
        public Guid Uid { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; }
        public string Question1 { get; set; }
        public string DescriptiveAnswer { get; set; }
        public int? TrueAnswerOptionId { get; set; }
        public string BlankQuestionAnswer { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
        public virtual ICollection<SubjectQuestion> SubjectQuestions { get; set; }
    }
}
