using PackageMagic.Nuget;

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
            List<Task> t = new List<Task>();
            var _projectDirectory = @"c:\git\tobias";
            //clsPackages.Callback += CallbackFromPackage;
            //PackageService.GetPackages(IMagickPackageType.Npm, Packa)

            var nuget = new Nuget();
            var npm = new Npm();

            t.Add(nuget.SearchPackages(_projectDirectory));
            t.Add(npm.SearchPackages(@"C:\git\tobias\package.json"));
            await Task.WhenAll(t);

            foreach (var item in nuget.GetPackages())
            {
                Console.WriteLine(item.Name);
            }
            foreach (var item in npm.GetPackages())
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
