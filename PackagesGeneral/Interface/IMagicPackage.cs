using PackageMagic.General.Type;

namespace PackageMagic.General.Interface
{
    public interface IMagicPackage
    {
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        MagicPackageType PackageType { get; set; }
    }
}
