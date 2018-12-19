using PackageMagic.PackageService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageService
{
    public class Packages
    {
        //public static ChangedCallback Callback { get; set; }
        //public static List<IMagicPackage> PackageInformation;
        //static Packages()
        //{
        //            PackageInformation = new List<IMagicPackage>();
            
        //}
        //public delegate void ChangedCallback(IMagicPackage package);
        //public static async Task AddToPackageInformation(IMagicPackage package)
        //{
        //    await Task.Run(() =>
        //    {
        //        if(package.Name == "Thinktecture.IdentityModel.Owin.ClientCertificates")
        //        {
        //            //
        //        }
        //        //Utils.LogMessages(package.Name);
        //        IMagicPackage checkIfExist = Packages.PackageInformation.Find(z => z.Name == package.Name && z.Version == package.Version);
        //        if (checkIfExist == null)
        //        {
        //           Utils.LogMessages($"Add: {package.Name}:{package.Version} - {package.PackageType}");
        //            PackageInformation.Add(package);

        //            Callback?.Invoke(package);
        //        }
        //        else
        //        {
        //            //Utils.LogMessages("Exists: " + package.Name);
        //        }

        //    });
        //}
    }
}
