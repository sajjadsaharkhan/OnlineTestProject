using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class Subject
    {
        public Subject()
        {
            InverseParent = new HashSet<Subject>();
            SubjectQuestions = new HashSet<SubjectQuestion>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual Subject Parent { get; set; }
        public virtual ICollection<Subject> InverseParent { get; set; }
        public virtual ICollection<SubjectQuestion> SubjectQuestions { get; set; }
    }
}
