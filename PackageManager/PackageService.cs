using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Interfaces
{
    public interface IMagicPackage
    {
        //string Id { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        IMagickPackageType PackageType { get; set; }



    }
    public interface IMagicPackageSearch
    {
        //string Id { get; set; }
        Task SearchPackages(string path);
        Task AddPackages(IMagicPackage package);
        IList<IMagicPackage> GetPackages();
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
