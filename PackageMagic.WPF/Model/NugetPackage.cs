using GalaSoft.MvvmLight;
using PackageMagic.WPF.Interface;

namespace PackageMagic.WPF.Model
{
    public class NugetPackage : ObservableObject, IMagicPackage
    {
        public string Id { get => _id; set => _id = value; }
        private string _id;

        public string Path { get => _path; set => _path = value; }
        private string _path;

        public PackageType Type { get => _type; set => _type = value; }
        private PackageType _type;
    }
}
