using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.WPF.Interface
{
    public delegate void StatusCallback(string status);

    public interface IMagicPackageService
    {
        StatusCallback MyCallback { get; set; }
        //Maybe one method for each package type, or a parameter to include different package types in the search?
        Task<IEnumerable<IMagicPackage>> GetPackagesAsync(string aPath);
    }
}
