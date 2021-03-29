using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class Exam
    {
        public Exam()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            ExamSubSections = new HashSet<ExamSubSection>();
            ExamUsers = new HashSet<ExamUser>();
        }

        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<ExamSubSection> ExamSubSections { get; set; }
        public virtual ICollection<ExamUser> ExamUsers { get; set; }
    }
}
