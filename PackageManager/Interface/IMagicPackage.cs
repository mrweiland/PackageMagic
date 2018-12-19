using PackageMagic.PackageService.Model;

namespace PackageMagic.PackageService.Interface
{
    public interface IMagicPackage
    {
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        MagicPackageType PackageType { get; set; }
    }
}
