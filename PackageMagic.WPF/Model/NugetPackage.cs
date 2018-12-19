using GalaSoft.MvvmLight;
using PackageMagic.WPF.Interface;

namespace PackageMagic.WPF.Model
{
    public class NugetPackage : ObservableObject, IMagicPackage
    {
        public string Id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Path { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public PackageType Type { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
