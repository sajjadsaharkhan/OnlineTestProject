using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class ExamSubSection
    {
        public ExamSubSection()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
        }

        public int Id { get; set; }
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
