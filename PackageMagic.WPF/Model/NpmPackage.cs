using GalaSoft.MvvmLight;
using PackageMagic.WPF.Interface;

namespace PackageMagic.WPF.Model
{
    public class NpmPackage : ObservableObject, IMagicPackage
    {
        public string Id
        {
            get => _id;
            set => Set(Id, ref _id, value);
        }
        private string _id;

        public string Path
        {
            get => _path;
            set => Set(Path, ref _path, value);
        }
        private string _path;

        //public PackageType Type
        //{
        //    get => _type;
        //    set => Set(()=>Type, ref _type, value);
        //}
        //private PackageType _type;

        public string IncludedInProject
        {
            get => _includedInProject;
            set => Set(IncludedInProject, ref _includedInProject, value);
        }
        private string _includedInProject;
    }
}
