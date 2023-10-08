using DAL;
using DAL.Models;
using DotNet_Task.IServices;

namespace DotNet_Task.Services
{
    public class CommonServices : ICommonServices
    {
        public CommonServices(AppDbContext context)
        {
            _context = context;
        }
        private readonly AppDbContext _context;
        public ProgramDetails? GetProgram(string programId)
        {
            return _context.ProgramDetails.FirstOrDefault(x => x.id.Equals(programId));
        }
    }
}
