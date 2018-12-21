using NuGet;
using PackageMagic.General;
using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageMagic.Nuget
{
    public class Nuget : IMagicPackageSearch
    {
        //New callback to the status field in the window
        public MessageDelegate MessageCallback { get; set; }

        private List<IMagicPackage> _packages;

        public Nuget()
        {
            _packages = new List<IMagicPackage>();
        }

        public async Task<IEnumerable<IMagicPackage>> SearchPackages(string path)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();

            //result.AddRange(await AddFromCsProj(path));
            result.AddRange(await AddFromPackagesConfig(path));

            return result;
        }

        /// <summary>
        /// Searches for packages configuration. (packages.config)
        /// </summary>
        /// <param name="projectFile">The project file.</param>
        /// <returns></returns>
        private async Task<IEnumerable<IMagicPackage>> AddFromPackagesConfig(string projectFile) => await Task.Run(() =>
        {
            List<NugetPackage> result = new List<NugetPackage>();

            var packagesFile = Path.Combine(Path.GetDirectoryName(projectFile), "packages.config");

            if (File.Exists(packagesFile))
            {
                var file = new PackageReferenceFile(packagesFile);

                foreach (PackageReference packageReference in file.GetPackageReferences())
                {
                    var package = new NugetPackage { Name = packageReference.Id, Version = packageReference.Version.ToNormalizedString(), Description = projectFile, PackageType = MagicPackageType.PackageConfig };
                    result.Add(package);
                }
            }

            return result;
        });

        /// <summary>
        /// Adds package references from CsProj/>
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <returns></returns>
        private async Task<IEnumerable<IMagicPackage>> AddFromCsProj(string projectFile) => await Task.Run(() =>
        {
            List<NugetPackage> result = new List<NugetPackage>();
            XmlDocument xDoc = new XmlDocument();
            string bareFilename = Path.GetFileName(projectFile);

            using (var fs = new FileStream(projectFile, FileMode.Open))
            {
                xDoc.Load(fs);
                bool isCore = false;

                isCore = IsNetCore(xDoc);

                if (!isCore)
                {
                    string targetFrameworkVersion = GetFrameworkVersion(xDoc);

                    //Then get normal references
                    var nodes = xDoc.GetElementsByTagName("Reference");
                    foreach (XmlNode node in nodes)
                    {
                        string packageVersion;
                        if (node.Attributes != null && node.Attributes.Count > 0)
                        {
                            string packageName = node.Attributes["Include"] == null ? "" : node.Attributes["Include"].Value;
                            //Castle.Core, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 407dd0808d44fbdc, processorArchitecture = MSIL
                            
                            if (packageName.Split(new char[] { ',' }).Count() > 1)
                            {
                                string[] temp = packageName.Split(new char[] { ',' });
                                packageName = temp[0];
                                packageVersion = temp[1].Split(new char[] { '=' })[1];
                            } else { 

                            

                            if (node.Attributes["Version"] == null)
                            {
                                    packageVersion = node.Attributes["version"] == null ? "" : node.Attributes["Version"].Value;
                            }
                            else
                            {
                                    //Console.WriteLine(node.Attributes["Version"].Value);
                                    packageVersion = node.Attributes["Version"] == null ? "" : node.Attributes["Version"].Value;
                            }

                            packageVersion = string.IsNullOrEmpty(packageVersion) ? targetFrameworkVersion : packageVersion;

                            }
                            result.Add(new NugetPackage { Name = packageName, Version = packageVersion, Description = "", PackageType = MagicPackageType.Reference });
                        }
                    }

                    //Then get new nuget references
                    nodes = xDoc.GetElementsByTagName("PackageReference");
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes != null && node.Attributes.Count > 0)
                        {
                            string packageVersion = "";
                            var packageName = node.Attributes["Include"].Value;
                            if (node.Attributes["Version"] != null)
                            {
                                packageVersion = node.Attributes["Version"].Value;
                            }
                            else
                            {
                                var versionNodes = node.SelectNodes("Version");
                                if (versionNodes != null && versionNodes.Count > 0)
                                {
                                    packageVersion = versionNodes[0].Value;
                                }
                            }
                            result.Add(new NugetPackage { Name = packageName, Version = packageVersion, Description = "", PackageType = MagicPackageType.PackageReference });
                        }
                    }
                }
            }

            return result;
        });

        private static string GetFrameworkVersion(XmlDocument xDoc)
        {
            var nodeFramework = xDoc.GetElementsByTagName("TargetFrameworkVersion");

            if (nodeFramework == null || nodeFramework.Count == 0)
                return "";

            return nodeFramework.Item(0).ChildNodes[0].Value ?? "";
        }

        private bool IsNetCore(XmlDocument xDoc)
        {
            //XmlElement root = xDoc.DocumentElement;
            //if (root.Attributes != null && root.Attributes.Count > 0)
            //{
            //    if (root.Attributes["Sdk"] != null && root.Attributes["Sdk"].Value == "Microsoft.NET.Sdk")
            //        return true;
            //    if (root.Attributes["Sdk"] != null && root.Attributes["Sdk"].Value == "Microsoft.NET.Sdk.Web")
            //        return true;
            //}
            return false;
        }






        /// <summary>
        /// Loads the nuget feed provided. Loops all packages and adds them to PackageInformation List.
        /// </summary>
        /// <param name="nugetFeedSourceV2">The nuget feed source v2.</param>
        /// <returns></returns>
        //public async Task LoadNugetFeed(string nugetFeedSourceV2) => await Task.Run(async () =>
        //{

        //    var repo = PackageRepositoryFactory.Default.CreateRepository(nugetFeedSourceV2);

        //    var packages = repo.GetPackages();
        //    foreach (var package1 in packages)
        //    {
        //        var pack = repo.FindPackage(package1.Id);

        //        //AddToPackageInformation(new NugetPackage { Name = pack.Id, Version = pack.Version.ToString(), Description = pack.Description, FeedRegistry = nugetFeedSourceV2, OriginOfPackage = NugetPackage.Origin.Nuget }).GetAwaiter().GetResult();
        //        await AddPackages(new NugetPackage { Name = pack.Id, Version = pack.Version.ToString(), Description = pack.Description, PackageType = IMagickPackageType.Nuget });
        //    }
        //});

        /// <summary>
        /// Searches the csproj files that is found. Searching for <Reference></Reference> and <PackageReference></PackageReference> in those files to support both new and old nuget format.
        /// </summary>
        /// <param name="file">The .csproj file.</param>
        /// <returns></returns>
        private async Task<IEnumerable<IMagicPackage>> SearchCsProj(string file) => await Task.Run(() =>
        {
            List<NugetPackage> result = new List<NugetPackage>();
            XmlDocument xDoc = new XmlDocument();

            using (var fs = new FileStream(file, FileMode.Open))
            {
                xDoc.Load(fs);
                var nodes = xDoc.GetElementsByTagName("PackageReference");
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes != null && node.Attributes.Count > 1)
                    {
                        //NugetPackages p = new NugetPackages();
                        Debug.WriteLine(node.Attributes["Include"].Value);
                        try
                        {
                            string packageVersion;
                            //p.PackageName = node.Attributes["Include"].Value;
                            if (node.Attributes["Version"] == null)
                            {
                                Debug.WriteLine(node.Attributes["version"].Value);
                                packageVersion = node.Attributes["version"].Value;
                            }
                            else
                            {
                                Debug.WriteLine(node.Attributes["Version"].Value);
                                packageVersion = node.Attributes["Version"].Value;
                            }
                            result.Add(new NugetPackage { Name = node.Attributes["Include"].Value, Version = packageVersion, Description = "", PackageType = MagicPackageType.PackageReference });
                            //await AddPackages(package);
                        }
                        catch (Exception)
                        {
                            //throw;
                        }
                    }
                }
            }
            return result;
        });

        // private async Task GetNugetPackageInformation() => await Task.Run(() =>
        //{
        //    IPackage pack;
        //    IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
        //    PackageService.Utils.LogMessages($"Checking nuget package information from https://packages.nuget.org/api/v2");

        //    foreach (var item in NugetPackage.PackageInformation.FindAll(o => o.PackageType == IMagickPackageType.PackageConfig || o.PackageType == IMagickPackageType.PackageReference))
        //    {
        //        pack = repo.FindPackage(item.Name, SemanticVersion.Parse(item.Version));
        //        ((NugetPackage)item).NugetPackageInformation = pack;
        //        PackageService.Utils.LogMessages($"Checking nuget package information  {item.Name}:{item.Version}");
        //        PackageService.Utils.LogMessages(pack.Id, true);
        //    }
        //});

        //public static async Task AddToPackageInformation(NugetPackage package)
        //{
        //    await Task.Run(() =>
        //    {

        //        Utils.LogMessages(package.Name);
        //        NugetPackage checkIfExist = NugetPackage.PackageInformation.Find(z => z.Name == package.Name && z.Version == package.Version);
        //        if (checkIfExist == null)
        //        {
        //            Utils.LogMessages($"Adding package: {package.Name}:{package.Version} - {package.Origin}");
        //            NugetPackage.PackageInformation.Add(package);

        //            Callback?.Invoke(package);
        //        }
        //        else
        //        {
        //            Utils.LogMessages("Exists: " + package.Name);
        //        }

        //    });
        //}
    }
}
