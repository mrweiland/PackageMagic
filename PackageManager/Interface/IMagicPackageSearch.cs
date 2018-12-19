using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Interface
{
    public interface IMagicPackageSearch
    {
        //string Id { get; set; }
        Task<IEnumerable<IMagicPackage>> SearchPackages(string path);
        Task AddPackages(IMagicPackage package);
        IList<IMagicPackage> GetPackages();
    }
}
