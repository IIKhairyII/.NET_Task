using DAL.DTOs.ApplicationTemplateDTOs;

namespace DAL.Models.ApplicationTemplate
{
    public class ApplicationTemplate
    {
        public string programId { get; set; }
        public PersonalInfoDTO? phone { get; set; }
        public PersonalInfoDTO? nationality { get; set; }
        public PersonalInfoDTO? residence { get; set; }
        public PersonalInfoDTO? idNumber { get; set; }
        public PersonalInfoDTO? birthDate { get; set; }
        public PersonalInfoDTO? gender { get; set; }
        public ProfileInfoDTO? education { get; set; }
        public ProfileInfoDTO? experience { get; set; }
        public ProfileInfoDTO? resume { get; set; }
        public List<Questions>? questions { get; set; }
    }
}
