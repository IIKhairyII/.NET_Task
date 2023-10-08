using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ApplicationTemplate
{
    public class QuestionsChoices
    {
        public List<string>? options { get; set; }
        public bool? enableOther { get; set; }
    }
}
