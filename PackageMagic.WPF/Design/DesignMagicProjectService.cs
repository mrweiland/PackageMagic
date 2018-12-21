using System.Collections.Generic;
using System.Threading.Tasks;
using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackageMagic.Nuget;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using PackagesNpm;

namespace PackageMagic.WPF.Design
{
    public class DesignMagicProjectService : IMagicProjectService
    {
        public MessageDelegate MessageCallback { get; set; }

        public async Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch) => await Task.Run(() =>
        {
            var result = new List<IMagicProject>();
            result.Add(new MagicProjectCs
            {
                Name = "Sample.csproj",
                Path = @"C:\SomePath\Sample.csproj",
                ProjectType = PackageService.Type.ProjectType.DOTNET,
                FrameworkVersion = "v4.7",
                Packages = new List<IMagicPackage>
                        {
                            new BasicPackage { Name = "System", PackageType=MagicPackageType.Reference},
                            new NugetPackage { Name = "GalaSoft.MvvmLight", PackageType=MagicPackageType.PackageReference},
                            new NugetPackage { Name = "NewtonSoft.Json",PackageType=MagicPackageType.PackageConfig},
                            new NpmPackage { Name = "AnyNpm",PackageType=MagicPackageType.Npm}
                        }
            });
            return result;
        });
    }
}