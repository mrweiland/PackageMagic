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
            var x = await nuget.SearchPackages(_projectDirectory);
            var y = await npm.SearchPackages(@"C:\git\tobias\package.json");



            foreach (var item in x)
            {
                Console.WriteLine(item.Name);
            }
            foreach (var item in y)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine($"NPM {y.Count()} Nuget {x.Count()} ");
            Console.ReadLine();
        }
    }
}
