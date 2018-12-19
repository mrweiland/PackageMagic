
    using NuGet;
using PackageMagic.PackageManager;
using System.Collections.Generic;

    namespace PackageMagic.Nuget.interfaces
    {

    //public delegate void ChangedCallback(IMagicPackage package);

    public class NugetPackage : IMagicPackage
        {
            //public string Id { get; set; }
            public string Name { get; set; }
            public string Version { get; set; }
            public static List<NugetPackage> PackageInformation;
            static NugetPackage()
            {
                //PackageInformation = new List<NugetPackage>();
            }

        public IPackage NugetPackageInformation { get; set; }

        public string Description { get; set; }
        public IMagickPackageType PackageType { get; set; }

        public IEnumerable<IMagickPackageType> Packages()
        {
            throw new System.NotImplementedException();
        }
    }

    }
