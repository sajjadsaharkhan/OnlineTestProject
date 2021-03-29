using System;
using System.Collections.Generic;

#nullable disable

namespace Project.Model.DataModels
{
    public partial class User
    {
        public User()
        {
            ExamUsers = new HashSet<ExamUser>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid Stamp { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<ExamUser> ExamUsers { get; set; }
    }
}
