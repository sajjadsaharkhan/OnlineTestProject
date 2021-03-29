using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class ExamQuestion
    {
        public ExamQuestion()
        {
            ExamUserAnswers = new HashSet<ExamUserAnswer>();
        }

        public int Id { get; set; }
        public int ExamId { get; set; }
        public int SubSectionId { get; set; }
        public int QuestionId { get; set; }
        public int Point { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual Question Question { get; set; }
        public virtual ExamSubSection SubSection { get; set; }
        public virtual ICollection<ExamUserAnswer> ExamUserAnswers { get; set; }
    }
}
