using PackageMagic.Nuget;
using PackageMagic.PackageManager;
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
            t.Add(clsNuget.SearchForPackagesConfig(_projectDirectory));
            t.Add(clsNuget.PopulatePackageReferences(_projectDirectory));
            t.Add(clsNpm.LoopPackageJson(@"C:\git\tobias\package.json"));
            await Task.WhenAll(t);
        }
    }
}
