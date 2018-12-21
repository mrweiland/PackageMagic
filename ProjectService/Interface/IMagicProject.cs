using PackageMagic.General.Interface;
using PackageMagic.ProjectService.Type;
using System.Collections.Generic;

namespace PackageMagic.ProjectService.Interface
{
    public interface IMagicProject
    {
        string Name { get; set; }

        string Path { get; set; }

        string FrameworkVersion { get; set; }

        FrameworkKind ProjectType { get; set; }

        List<IMagicPackage> Packages { get; set; }
    }
}