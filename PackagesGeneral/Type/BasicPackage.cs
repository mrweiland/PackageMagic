using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageMagic.General.Interface;

namespace PackageMagic.General.Type
{
    public class BasicPackage : IMagicPackage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public static List<IMagicPackage> PackageInformation = new List<IMagicPackage>();
        public string Description { get; set; }
        public MagicPackageType PackageType { get; set; }
    }
}
