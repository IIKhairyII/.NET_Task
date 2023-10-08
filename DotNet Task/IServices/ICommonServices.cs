using DAL.Models;

namespace DotNet_Task.IServices
{
    public interface ICommonServices
    {
        public ProgramDetails GetProgram(string programId);
    }
}
