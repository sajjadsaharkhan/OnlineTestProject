using OnlineTest.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.ViewModels
{
    public class QuestionWithAnswerViewModel
    {
        public Guid Uid { get; set; }
        public QuestionTypes Type { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string DescriptiveAnswer { get; set; }
    }
}
