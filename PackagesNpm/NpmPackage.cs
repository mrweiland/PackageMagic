﻿using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackagesNpm
{
    public class NpmPackage : IMagicPackage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public static List<IMagicPackage> PackageInformation = new List<IMagicPackage>();
        public string Description { get; set; }
        public PackageKind PackageType { get; set; }
    }
}
