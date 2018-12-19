using PackageMagic.General.Interface;
using System.Collections.Generic;

namespace PackageMagic.PackageService.Interface
{
    public interface IMagicProject
    {
        string Name { get; set; }
        string Path { get; set; }
        List<IMagicPackage> Packages { get; set; }
    }
}