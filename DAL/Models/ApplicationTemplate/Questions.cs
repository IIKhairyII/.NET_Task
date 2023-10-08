using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ApplicationTemplate
{
    public class Questions
    {
        public int type { get; set; }
        public string? question { get; set; }
        public byte questionFor { get; set; } //Personal, Profile Infos or an additional question

        protected enum questionSection
        {
            personal = 1,
            profile = 2,
            additional = 0
        }
        protected enum questionType
        {
            paragraph,
            shortAnswer,
            yesNo,
            dropDown,
            multiChoice,
            date,
            number
        }
    }
}
