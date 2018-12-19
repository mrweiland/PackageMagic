using System.Collections.Generic;
using System.Threading.Tasks;
using PackageMagic.WPF.Interface;

namespace PackageMagic.WPF.Service
{
    public class PackageService : IPackageService
    {
        private string _path;
        private List<IMagicPackage> _packages;

        public async Task<IEnumerable<IMagicPackage>> GetPackagesAsync(string aPath) => await Task.Run(() =>
        {
            //Do the magic to get all kinds of packages
            //If this method includes any awaitable async call you can remove the 'await Task.Run' and make it to a normal method
            //This is only made to make a non async method to behave as an async method against the outer world

            return new List<IMagicPackage>();
        });
    }
}
