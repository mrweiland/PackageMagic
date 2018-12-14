using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageManager
{
    public interface IPackageMagic
    {
        string Id { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        string Origin { get; set; }
    }

    //public enum Origin
    //{
    //    Npm,
    //    Nuget,
    //    PackageReference,
    //    Reference,
    //    PackageConfig
    //}
    //public class PackageMagic
    //{
    //}
}
