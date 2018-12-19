using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using PackageMagic.PackageService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Model
{
    public class MagicProjectCs : MagicProjectBase
    {
    }

    public abstract class MagicProjectBase : IMagicProject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<IMagicPackage> Packages { get; set; } = new List<IMagicPackage>();
    }
}
