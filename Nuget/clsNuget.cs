

using NuGet;
using PackageMagic.Nuget.interfaces;
using PackageMagic.PackageManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageMagic.Nuget
{
    public class clsNuget
    {
        public static ChangedCallback Callback { get; set; }

        /// <summary>
        /// Loads the nuget feed provided. Loops all packages and adds them to PackageInformation List.
        /// </summary>
        /// <param name="nugetFeedSourceV2">The nuget feed source v2.</param>
        /// <returns></returns>
        public static async Task LoadNugetFeed(string nugetFeedSourceV2)
        {
            await Task.Run(() =>
            {
                var repo = PackageRepositoryFactory.Default.CreateRepository(nugetFeedSourceV2);

                var packages = repo.GetPackages();
                foreach (var package1 in packages)
                {
                    var pack = repo.FindPackage(package1.Id);

                    //AddToPackageInformation(new NugetPackage { Name = pack.Id, Version = pack.Version.ToString(), Description = pack.Description, FeedRegistry = nugetFeedSourceV2, OriginOfPackage = NugetPackage.Origin.Nuget }).GetAwaiter().GetResult();
                    AddToPackageInformation(new NugetPackage { Name = pack.Id, Version = pack.Version.ToString(), Description = pack.Description, Origin= "Nuget" }).GetAwaiter().GetResult();
                }
            });
        }

        /// <summary>
        /// Searches the csproj files that is found. Searching for <Reference></Reference> and <PackageReference></PackageReference> in those files to support both new and old nuget format.
        /// </summary>
        /// <param name="file">The .csproj file.</param>
        /// <returns></returns>
        public static async Task SearchCsProj(string file)
        {

            XmlDocument xDoc = new XmlDocument();
            //Utils.LogMessages($"CS Proj file {file}", true);
            using (var fs = new FileStream(file, FileMode.Open))
            {
                xDoc.Load(fs);

                // Load Xml

                var nodes = xDoc.GetElementsByTagName("PackageReference");

                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes != null && node.Attributes.Count >1)
                    {
                        //NugetPackages p = new NugetPackages();
                        //Console.WriteLine(node.Attributes["Include"].Value);
                        try
                        {

                            string packageVersion;
                            //p.PackageName = node.Attributes["Include"].Value;


                            if (node.Attributes["Version"] == null)
                            {
                                //Console.WriteLine(node.Attributes["version"].Value);
                                packageVersion = node.Attributes["version"].Value;
                            }
                            else
                            {
                                //Console.WriteLine(node.Attributes["Version"].Value);
                                packageVersion = node.Attributes["Version"].Value;
                            }


                            await AddToPackageInformation(new NugetPackage { Name = node.Attributes["Include"].Value, Version = packageVersion, Description = "", Origin = "PackageReference" });
                        }
                        catch (Exception)
                        {

                            //throw;
                        }
                    }

                }

                //var nodes2 = xDoc.GetElementsByTagName("Reference");

                //foreach (XmlNode node in nodes2)
                //{
                //    if (node.Attributes != null)
                //    {

                //        string[] version = node.Attributes["Include"].Value.Trim().Split(',');
                //        if (version.Length > 2)
                //        {
                //            var foundVersion = version[1].Trim().Replace("Version=", "");
                //            await Utils.AddToPackageInformation(new PackageInformation { PackageName = version[0], PackageVersion = foundVersion, PackageDescription = "", OriginOfPackage = PackageInformation.Origin.Reference });
                //        }
                //    }
                //}


            }


        }

        /// <summary>
        /// Searches for packages configuration. (packages.config)
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <returns></returns>
        public static async Task SearchForPackagesConfig(string projectDirectory)
        {
            Utils.LogMessages($"Search package config {projectDirectory}", true);
            await Task.Run(() =>
            {
                string[] packagesConfig = Directory.GetFiles(projectDirectory, "packages.config", SearchOption.AllDirectories);

                foreach (var packConfig in packagesConfig)
                {
                    //NugetPackages p = new NugetPackages();
                    var directoryName = Path.GetDirectoryName(packConfig);
                    Utils.LogMessages(directoryName);
                    string foundCsProjFile = "";
                    if (directoryName != null)
                    {
                        string[] proj = Directory.GetFiles(directoryName, "*.csproj", SearchOption.TopDirectoryOnly);

                        if (proj.Length > 0)
                        {
                            foundCsProjFile = proj[0];
                        }
                        else
                        {
                            foundCsProjFile = "No csProj file found " + packConfig;
                        }
                    }

                    #region MyRegion

                    //xml = new XmlSerializer(typeof(Packages));
                    //reader = new StreamReader(packConfig);
                    //settings = (Packages)xml.Deserialize(reader);

                    ////var file = new PackageReferenceFile(packConfig);
                    //foreach (var packageReference in settings.Package)
                    //{

                    //    Utils.LogMessages(packageReference, true);

                    //    //await Utils.AddToPackageInformation(new PackageInformation { PackageName = packageReference.Id, PackageVersion = packageReference.Version.ToNormalizedString(), PackageDescription = foundCsProjFile, OriginOfPackage = PackageInformation.Origin.PackageConfig, CsProjFile = foundCsProjFile });
                    //    Utils.AddToPackageInformation(new PackageInformation { PackageName = packageReference.Id, PackageVersion = packageReference.Version.to, PackageDescription = foundCsProjFile, OriginOfPackage = PackageInformation.Origin.PackageConfig, CsProjFile = foundCsProjFile }).GetAwaiter().GetResult();

                    //}

                    #endregion

                    var file = new PackageReferenceFile(packConfig);
                    foreach (PackageReference packageReference in file.GetPackageReferences())
                    {

                        Utils.LogMessages(packageReference.Id, true);

                        var package = new NugetPackage { Name = packageReference.Id, Version = packageReference.Version.ToNormalizedString(), Description = foundCsProjFile, Origin = "PackageConfig" };

                        AddToPackageInformation(package).GetAwaiter().GetResult();
                    }
                }
            });
        }

        public static async Task GetNugetPackageInformation()
        {
            IPackage pack;
            IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            Utils.LogMessages($"Checking nuget package information from https://packages.nuget.org/api/v2");
            await Task.Run(() =>
            {
                foreach (var item in NugetPackage.PackageInformation.FindAll(o => o.Origin == "PackageConfig" || o.Origin == "PackageReference"))
                {
                    pack = repo.FindPackage(item.Name, SemanticVersion.Parse(item.Version));
                    item.NugetPackageInformation = pack;
                    Utils.LogMessages($"Checking nuget package information  {item.Name}:{item.Version}");
                    Utils.LogMessages(pack.Id, true);
                }
            });

        }

        /// <summary>
        /// Populates the package references and calls <see cref="SearchCsProj"/>
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <returns></returns>
        public static async Task PopulatePackageReferences(string projectDirectory)
        {

            string[] csProjFiles = Directory.GetFiles(projectDirectory, "*.csproj", SearchOption.AllDirectories);
            foreach (var csProjFile in csProjFiles)
            {
                await SearchCsProj(csProjFile);
            }

        }

        public static async Task AddToPackageInformation(NugetPackage package)
        {
            await Task.Run(() =>
            {
                
                Utils.LogMessages(package.Name);
                NugetPackage checkIfExist = NugetPackage.PackageInformation.Find(z => z.Name == package.Name && z.Version == package.Version);
                if (checkIfExist == null)
                {
                    Utils.LogMessages($"Adding package: {package.Name}:{package.Version} - {package.Origin}");
                    NugetPackage.PackageInformation.Add(package);

                    Callback?.Invoke(package);
                }
                else
                {
                    Utils.LogMessages("Exists: " + package.Name);
                }

            });
        }


    }
}
