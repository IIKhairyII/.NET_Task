namespace DAL.Models
{
    public class WorkflowStages
    {
        public string programId { get; set; }
        public string? stageId { get; set; }
        public string stageName { get; set; }
        public string stageType { get; set; }
        public bool isShownToCandidate { get; set; }
        public VideoInterviewsQuestionsInfo? questions { get; set; }
    }
}
