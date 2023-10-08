using DAL;
using DotNet_Task.IServices;
using DotNet_Task.Services;

namespace DotNet_Task
{
    public static class DependenciesInjected
    {
        public static void InitAppDependencies(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();
            services.AddScoped<ICommonServices, CommonServices>();
        }
    }
}
