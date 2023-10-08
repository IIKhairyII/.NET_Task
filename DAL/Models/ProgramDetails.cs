using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ProgramDetails
    {
        public string? id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string? summary { get; set; }
        public string description { get; set; }
        public List<string>? skills { get; set; }
        public string? benefits { get; set; }
        public string? criteria { get; set; }
        public string? duration { get; set; }
        public string location { get; set; }
        public bool? isRemote { get; set; }
        public int maxApplicants { get; set; }
        public string? minQualifications { get; set; }
        public DateTime? programStart { get; set; }
        public DateTime applicationStart { get; set; }
        public DateTime applicationEnd { get; set; }
        public List<WorkflowStages>? WorkflowStages { get; set; }
    }
}
