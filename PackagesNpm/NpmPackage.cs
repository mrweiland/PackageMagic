using PackageMagic.PackageManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesNpm
{
    public class NpmPackage : IPackageMagic
    {
        //public delegate void ChangedCallback(IPackageMagic package);


            public string Id { get; set; }
            public string Name { get; set; }
            public string Origin { get; set; }
            public string Version { get; set; }
            public static List<NpmPackage> PackageInformation;
            static NpmPackage()
            {
                //PackageInformation = new List<NpmPackage>();
            }


            public string Description { get; set; }


        }

}
