using System.Collections.Generic;
using System.Threading.Tasks;
using PackageMagic.General.Type;

namespace PackageMagic.PackageService.Interface
{
    public interface IMagicProjectService
    {
        MessageDelegate MessageCallback { get; set; }
        Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch);
    }
}