using PackageMagic.General.Interface;
using PackageMagic.ProjectService.Interface;
using PackageMagic.ProjectService.Model;
using PackageMagic.ProjectService.Service;
using PackageMagic.ProjectService.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.ProjectService.Model
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
