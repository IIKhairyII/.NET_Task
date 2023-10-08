using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class VideoInterviewsQuestionsInfo
    {
        public string? question { get; set; }
        public string? videoInfo { get; set; }
        public string? videoDuration { get; set; }
        public string? durationUnit { get; set; }
        public string? deadline { get; set; }
    }
}
