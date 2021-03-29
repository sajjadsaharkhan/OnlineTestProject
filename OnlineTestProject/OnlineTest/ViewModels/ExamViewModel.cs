using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.ViewModels
{
    public class ExamViewModel
    {
        public Guid Uid { get; set; }
        public string Title { get; set; }
        public List<ExamSubSectionViewModel> SubSections { get; set; }
    }
}
