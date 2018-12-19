using System.Collections.Generic;
using System.Threading.Tasks;
using PackageMagic.General.Type;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;

namespace PackageMagic.WPF.Design
{
    public class DesignMagicProjectService : IMagicProjectService
    {
        public MessageDelegate MessageCallback { get; set; }

        public async Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch) => await Task.Run(() =>
        {
            return new List<IMagicProject>();
        });
    }
}