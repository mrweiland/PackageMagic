﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackageMagic.Nuget;
using PackageMagic.ProjectService.Interface;
using PackageMagic.ProjectService.Model;
using PackageMagic.ProjectService.Type;
using PackagesNpm;

namespace PackageMagic.WPF.Design
{
    public class DesignMagicProjectService : IMagicProjectService
    {
        public MessageDelegate MessageCallback { get; set; }

        public async Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch, ProjectKind projectKind) => await Task.Run(() =>
        {
            var result = new List<IMagicProject>
            {
                new MagicProjectCs(@"C:\SomePath\Sample1.csproj")
                {
                    ProjectType = FrameworkKind.DOTNET,
                    FrameworkVersion = "v4.7",
                    Packages = new List<IMagicPackage>
                        {
                            new BasicPackage { Name = "System", PackageType=PackageKind.Reference},
                            new NugetPackage { Name = "GalaSoft.MvvmLight", PackageType=PackageKind.PackageReference},
                            new NugetPackage { Name = "NewtonSoft.Json",PackageType=PackageKind.PackageConfig},
                            new NpmPackage { Name = "AnyNpm",PackageType=PackageKind.Npm}
                        }
                },
                new MagicProjectCs(@"C:\SomePath\Sample2.csproj")
                {
                    ProjectType = FrameworkKind.DOTNET,
                    FrameworkVersion = "v4.7",
                    Packages = new List<IMagicPackage>
                        {
                            new BasicPackage { Name = "System", PackageType=PackageKind.Reference},
                            new NugetPackage { Name = "GalaSoft.MvvmLight", PackageType=PackageKind.PackageReference},
                            new NugetPackage { Name = "NewtonSoft.Json",PackageType=PackageKind.PackageConfig},
                            new NpmPackage { Name = "AnyNpm",PackageType=PackageKind.Npm}
                        }
                }
            };
            return result;
        });
    }
}