using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageManager
{
    public interface IMagicPackage
    {
        //string Id { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        IMagickPackageType PackageType { get; set; }

        IEnumerable<IMagickPackageType> Packages();
     


        
    }

    public enum IMagickPackageType
    {
        Npm,
        Nuget,
        PackageReference,
        Reference,
        PackageConfig
    }
    //public class PackageMagic
    //{
    //}
}
