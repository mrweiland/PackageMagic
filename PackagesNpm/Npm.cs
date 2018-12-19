using Newtonsoft.Json.Linq;
using PackageMagic.PackageService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PackagesNpm
{
    public class Npm : IMagicPackageSearch
    {
        public static readonly string NpmPath;
        public static string NpmRegistry { get; set; }
        private List<IMagicPackage> _packages;

        public Npm()
        {
            _packages = new List<IMagicPackage>();
            
        }

        public IList<IMagicPackage> GetPackages()
        {
            return _packages;
        }

        public async Task AddPackages(IMagicPackage package) => await Task.Run(() =>
        {
            IMagicPackage checkIfExist = _packages.Find(z => z.Name == package.Name && z.Version == package.Version);
            if (checkIfExist == null)
            {
                _packages.Add(package);
            }


        });

        static Npm()
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

        public  async Task SearchPackages(string packageJsonPath)
        {
            dynamic o1 = JObject.Parse(File.ReadAllText(packageJsonPath));
            IList<JToken> jsonDevDep = o1["devDependencies"];
            if (jsonDevDep != null)
            {
                foreach (var jToken in jsonDevDep)
                {
                    var p = (JProperty)jToken;
                    //var license = await RunNpmViewCheckLicense(p.Name, p.Value.ToString());
                    await AddPackages(new NpmPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = IMagickPackageType.Npm });
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
                    await AddPackages(new NpmPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = IMagickPackageType.Npm });
                }


            }
        }

  
    }
}
