using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class ExamUser
    {
        public ExamUser()
        {
            ExamUserAnswers = new HashSet<ExamUserAnswer>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public bool IsParticipated { get; set; }
        public DateTime? ExamStartDate { get; set; }
        public DateTime? ExamEndDate { get; set; }
        public bool IsFinilized { get; set; }
        public bool IsEnded { get; set; }
        public int Point { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ExamUserAnswer> ExamUserAnswers { get; set; }
    }
}
