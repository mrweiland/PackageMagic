using PackageMagic.Nuget;
using PackageMagic.ProjectService.Interface;
using PackageMagic.ProjectService.Service;
using PackageMagic.ProjectService.Type;
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

            var Projects = await _projectService.GetProjectsAsync(@"C:\git\tobias", ProjectKind.CSharp);

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
