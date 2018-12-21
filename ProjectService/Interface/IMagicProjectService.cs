using System.Collections.Generic;
using System.Threading.Tasks;
using PackageMagic.General.Type;
using PackageMagic.ProjectService.Type;

namespace PackageMagic.ProjectService.Interface
{
    public interface IMagicProjectService
    {
        MessageDelegate MessageCallback { get; set; }
        Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch, ProjectKind projectKind);
    }
}