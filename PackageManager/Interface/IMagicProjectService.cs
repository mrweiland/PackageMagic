using PackageMagic.General.Type;
using PackageMagic.PackageService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Interface
{
    public interface IMagicProjectService
    {
        MessageDelegate MessageCallback { get; set; }
        Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch);
    }
}