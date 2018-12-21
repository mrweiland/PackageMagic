using PackageMagic.General.Interface;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using PackageMagic.PackageService.Service;
using PackageMagic.PackageService.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Model
{
    public abstract class MagicProjectBase : IMagicProject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FrameworkVersion { get; set; }
        public FrameworkKind ProjectType { get; set; }
        public List<IMagicPackage> Packages { get; set; } = new List<IMagicPackage>();

        public abstract Task ParseAsync();
    }
}
