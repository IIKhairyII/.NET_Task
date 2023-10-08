namespace DAL.Models.ApplicationTemplate
{
    public class Questions
    {
        public byte? type { get; set; }
        public string? question { get; set; }
        public byte? questionForSection { get; set; } //Personal, Profile Infos or an additional question
        public QuestionsChoices? choices { get; set; }

        public enum questionSection
        {
            personal = 1,
            profile = 2,
            additional = 0
        }
        public  enum questionType
        {
            paragraph,
            shortAnswer,
            date,
            number,
            dropDown = 10,
            multiChoice = 11,
            yesNo= 12
        }
    }
}
