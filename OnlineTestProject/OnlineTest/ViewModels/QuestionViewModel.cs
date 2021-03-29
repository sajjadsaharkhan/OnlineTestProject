using OnlineTest.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Uid { get; set; }
        public QuestionTypes Type { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public List<QuestionOptionsViewModel> Options { get; set; }
    }
}
