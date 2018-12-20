using PackageMagic.Nuget;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Service;
using PackagesNpm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRunner
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Runner().GetAwaiter().GetResult();
        }

         static async Task Runner()
        {
            MagicProjectService _projectService = new MagicProjectService();

            //clsPackages.Callback += CallbackFromPackage;
            //PackageService.GetPackages(IMagickPackageType.Npm, Packa)

            var Projects = await _projectService.GetProjectsAsync(@"C:\git\tobias");

            foreach (var project in Projects)
            {
                Console.WriteLine($"{project.Name}");
                foreach (var package in project.Packages)
                {
                    Console.WriteLine($"- {package.Name}");
                }
            }
            Console.WriteLine(Projects.Count());
            Console.ReadLine();
        }
    }
}
