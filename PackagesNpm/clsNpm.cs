using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageMagic.PackageManager;

namespace PackagesNpm
{
    public class clsNpm
    {
        public static readonly string NpmPath;
        public static string NpmRegistry { get; set; }
        static clsNpm()
        {
            NpmPath = FindNpmPath("npm.cmd");
        }
        private static string FindNpmPath(string npmCmd)
        {
            npmCmd = Environment.ExpandEnvironmentVariables(npmCmd);
            if (!File.Exists(npmCmd))
            {
                if (Path.GetDirectoryName(npmCmd) == string.Empty)
                    foreach (var test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'))
                    {
                        var path = test.Trim();
                        if (!string.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, npmCmd)))
                            return Path.GetFullPath(path);
                    }

                throw new FileNotFoundException(new FileNotFoundException().Message, npmCmd);
            }

            return Path.GetFullPath(npmCmd);
        }

        public static async Task LoopPackageJson(string packageJsonPath)
        {
            dynamic o1 = JObject.Parse(File.ReadAllText(packageJsonPath));
            IList<JToken> jsonDevDep = o1["devDependencies"];
            if (jsonDevDep != null)
            {
                foreach (var jToken in jsonDevDep)
                {
                    var p = (JProperty)jToken;
                    //var license = await RunNpmViewCheckLicense(p.Name, p.Value.ToString());
                    await clsPackages.AddToPackageInformation(new NpmPackage { Name= p.Name, Version = p.Value.ToString(), Description= "", PackageType= IMagickPackageType.Npm});
                }
            }
            IList<JToken> jsonDep = o1["dependencies"];
            if (jsonDep != null)
            {
                foreach (var jToken in jsonDep)
                {
                    var p = (JProperty)jToken;
                    //var license = await RunNpmViewCheckLicense(p.Name, p.Value.ToString());
                    //await Utils.AddToPackageInformation(new PackageInformation { PackageName = p.Name, PackageVersion = p.Value.ToString(), PackageDescription = "", OriginOfPackage = PackageInformation.Origin.Npm, FeedRegistry = NpmRegistry });
                    await clsPackages.AddToPackageInformation(new NpmPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = IMagickPackageType.Npm });
                }


            }
        }
    }
}
