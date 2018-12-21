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
    public class MagicProjectCs : MagicProjectBase
    {
        //Add anything special for this type of project
        public async Task Parse() => await Task.Run(() =>
        {
            var parser = CsProjParser.Parse(Path);
            FrameworkVersion = parser.FrameworkVersion;
            ProjectType = parser.ProjectType;
            Packages.AddRange(parser.Packages);
        });
    }
    public class MagicProjectNpm : MagicProjectBase
    {
        //Add anything special for this type of project
        public async Task Parse() => await Task.Run(() =>
        {
            var parser = NpmProjParser.Parse(Path);
            Packages.AddRange(parser.Packages);
        });
    }

    public class MagicProjectVb : MagicProjectBase
    {
        //Add anything special for this type of project
    }

    public abstract class MagicProjectBase : IMagicProject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FrameworkVersion { get; set; }
        public ProjectType ProjectType { get; set; }
        public List<IMagicPackage> Packages { get; set; } = new List<IMagicPackage>();
    }
}
