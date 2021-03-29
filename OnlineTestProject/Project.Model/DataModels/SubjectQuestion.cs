using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class SubjectQuestion
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int SubjectId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
