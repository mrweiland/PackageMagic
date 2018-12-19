
using NuGet;

using PackageMagic.PackageService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.Nuget
    {

    //public delegate void ChangedCallback(IMagicPackage package);

    public class NugetPackage : IMagicPackage
        {
            //public string Id { get; set; }
            public string Name { get; set; }
            public string Version { get; set; }
            public static List<IMagicPackage> PackageInformation;
            static NugetPackage()
            {
                PackageInformation = new List<IMagicPackage>();
            }

        public IPackage NugetPackageInformation { get; set; }

        public string Description { get; set; }
        public IMagickPackageType PackageType { get; set; }




    }

    }
