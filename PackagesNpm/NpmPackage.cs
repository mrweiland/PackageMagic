using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesNpm
{
    public class NpmPackage : IMagicPackage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public static List<IMagicPackage> PackageInformation = new List<IMagicPackage>();
        public string Description { get; set; }
        public MagicPackageType PackageType { get; set; }
    }
}
