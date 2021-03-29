using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class QuestionOption
    {
        public QuestionOption()
        {
            ExamUserAnswers = new HashSet<ExamUserAnswer>();
        }

        public int Id { get; set; }
        public Guid Uid { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<ExamUserAnswer> ExamUserAnswers { get; set; }
    }
}
