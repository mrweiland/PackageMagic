using PackageMagic.WPF.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.WPF.Design
{
    //public class DesignPackageService : IMagicPackageService
    //{
    //    public StatusCallback MyCallback { get; set; }

    //    //private readonly string _path;
    //    //private readonly List<IMagicPackage> _packages;

    //    public async Task<IEnumerable<IMagicPackage>> GetPackagesAsync(string aPath) => await Task.Run(() =>
    //    {
    //        //Do the magic to get all kinds of packages
    //        //If this method includes any awaitable async call you can remove the 'await Task.Run' and make it to a normal method
    //        //This is only made to make a non async method to behave as an async method against the outer world
    //        List<IMagicPackage> result = new List<IMagicPackage>
    //        {
    //            new InternalNugetPackage { Id = "GalaSoft.MvvmLight", Path = @"C:\Repos\Nuget"},
    //            new ExternalNugetPackage { Id = "NewtonSoft.Json", Path = @"C:\Repos\Nuget"},
    //            new NpmPackage { Id = "AnyNpm", Path = @"C:\Repos\Npm"}
    //        };
    //        return result;
    //    });
    //}
}
