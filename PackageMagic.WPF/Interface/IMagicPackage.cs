using PackageMagic.WPF.Model;

namespace PackageMagic.WPF.Interface
{
    public interface IMagicPackage
    {
        //Add common properties and methods for all package types here
        string Id { get; set; }
        string Path { get; set; }
        PackageType Type { get; set; }
    }
}
