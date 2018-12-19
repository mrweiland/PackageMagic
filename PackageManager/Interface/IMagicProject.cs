using PackageMagic.General.Interface;
using System.Collections.Generic;

namespace PackageMagic.PackageService.Interface
{
    public interface IMagicProject
    {
        string Name { get; set; }

        string Path { get; set; }

        //TODO! Could be nice to have this value, maybe have a separate csproj-parser 
        //that could lift out some basic information about the project? 
        //In that case we could start already at project level to read the xml into a stream
        //and then feed the Nuget class with that stream so it could continue to parse references
        string FrameworkVersion { get; set; }

        List<IMagicPackage> Packages { get; set; }
    }
}