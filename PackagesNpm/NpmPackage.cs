
using PackageMagic.PackageService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesNpm
{
    public class NpmPackage : IMagicPackage
    {
        //public delegate void ChangedCallback(IMagicPackage package);


            public string Id { get; set; }
            public string Name { get; set; }
            public string Version { get; set; }
            public static List<IMagicPackage> PackageInformation;
            static NpmPackage()
            {
                PackageInformation = new List<IMagicPackage>();
            }


            public string Description { get; set; }

        public IMagickPackageType PackageType { get; set; }

  


    }

}
