using NuGet;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.Nuget
{
    public class NugetPackage : IMagicPackage
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public static List<IMagicPackage> PackageInformation = new List<IMagicPackage>();
        public IPackage NugetPackageInformation { get; set; }
        public string Description { get; set; }
        public MagicPackageType PackageType { get; set; }
    }
}
