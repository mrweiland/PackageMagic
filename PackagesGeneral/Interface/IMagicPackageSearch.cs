using PackageMagic.General.Type;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.General.Interface
{
    public interface IMagicPackageSearch
    {
        //New callback to the status field in the window
        MessageDelegate MessageCallback { get; set; }
        //string Id { get; set; }
        Task<IEnumerable<IMagicPackage>> SearchPackages(string path);
        //Task AddPackages(IMagicPackage package);
        //IList<IMagicPackage> GetPackages();
    }
}
