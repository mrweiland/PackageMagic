﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageManager
{
    public class clsPackages
    {
        public static ChangedCallback Callback { get; set; }
        public static List<IPackageMagic> PackageInformation;
        static clsPackages()
        {
                    PackageInformation = new List<IPackageMagic>();
            
        }
        public delegate void ChangedCallback(IPackageMagic package);
        public static async Task AddToPackageInformation(IPackageMagic package)
        {
            await Task.Run(() =>
            {

                //Utils.LogMessages(package.Name);
                IPackageMagic checkIfExist = clsPackages.PackageInformation.Find(z => z.Name == package.Name && z.Version == package.Version);
                if (checkIfExist == null)
                {
                    //Utils.LogMessages($"Adding package: {package.Name}:{package.Version} - {package.Origin}");
                    clsPackages.PackageInformation.Add(package);

                    Callback?.Invoke(package);
                }
                else
                {
                    //Utils.LogMessages("Exists: " + package.Name);
                }

            });
        }
    }
}
