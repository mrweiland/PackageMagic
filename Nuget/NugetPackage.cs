
    using NuGet;
using PackageMagic.PackageManager;
using System.Collections.Generic;

    namespace PackageMagic.Nuget.interfaces
    {

        //public delegate void ChangedCallback(IPackageMagic package);

        public class NugetPackage: IPackageMagic
        {
            //public string Id { get; set; }
            public string Name { get; set; }
            public string Origin { get; set; }
            public string Version { get; set; }
            public static List<NugetPackage> PackageInformation;
            static NugetPackage()
            {
                //PackageInformation = new List<NugetPackage>();
            }

        public IPackage NugetPackageInformation { get; set; }

        public string Description { get; set; }
            

        }

    }
