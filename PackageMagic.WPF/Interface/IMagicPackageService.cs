﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.WPF.Interface
{
    public interface IMagicPackageService
    {
        //Maybe one method for each package type, or a parameter to include different package types in the search?
        Task<IEnumerable<IMagicPackage>> GetPackagesAsync(string aPath);
    }
}