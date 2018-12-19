using NuGet;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackageMagic.Nuget
{
    public class Nuget : IMagicPackageSearch
    {
        //public static ChangedCallback Callback { get; set; }
        public List<IMagicPackage> _packages;
        public Nuget()
        {
            _packages = new List<IMagicPackage>();

        }
        public async Task AddPackages(IMagicPackage package) => await Task.Run(() =>
        {
            //IMagicPackage checkIfExist = _packages.Find(z => z.Name == package.Name && z.Version == package.Version);
            //if (checkIfExist == null)
            //{
            _packages.Add(package);
            //}

        });

        public IList<IMagicPackage> GetPackages()
        {
            return _packages;
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
            List<NugetPackage> temp = new List<NugetPackage>();
            XmlDocument xDoc = new XmlDocument();

            using (var fs = new FileStream(file, FileMode.Open))
            {

                xDoc.Load(fs);

                // Load Xml
                var nodes = xDoc.GetElementsByTagName("PackageReference");
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes != null && node.Attributes.Count > 1)
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


                            temp.Add(new NugetPackage { Name = node.Attributes["Include"].Value, Version = packageVersion, Description = "", PackageType = MagicPackageType.PackageReference });

                            //await AddPackages(package);


                        }
                        catch (Exception)
                        {

                            //throw;
                        }
                    }


                }

            }
            return temp;


        });

        /// <summary>
        /// Searches for packages configuration. (packages.config)
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <returns></returns>
        private async Task<IEnumerable<IMagicPackage>> SearchForPackagesConfig(string projectDirectory) => await Task.Run(() =>
       {

           List<NugetPackage> temp = new List<NugetPackage>();
           PackageService.Utils.LogMessages($"Search package config {projectDirectory}", true);

           string[] packagesConfig = Directory.GetFiles(projectDirectory, "packages.config", SearchOption.AllDirectories);

           foreach (var packConfig in packagesConfig)
           {
                //NugetPackages p = new NugetPackages();
                var directoryName = Path.GetDirectoryName(packConfig);
               PackageService.Utils.LogMessages(directoryName);
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

                   PackageService.Utils.LogMessages(packageReference.Id, true);

                   var package = new NugetPackage { Name = packageReference.Id, Version = packageReference.Version.ToNormalizedString(), Description = foundCsProjFile, PackageType = MagicPackageType.PackageConfig };

                    //await AddPackages(package);
                    temp.Add(package);
               }
           }
           return temp;
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


        public async Task<IEnumerable<IMagicPackage>> SearchPackages(string path)
        {
            List<IMagicPackage> tempList = new List<IMagicPackage>();
            tempList.AddRange(await PopulatePackageReferences(path));
            tempList.AddRange(await SearchForPackagesConfig(path));

            return tempList;
        }
        /// <summary>
        /// Populates the package references and calls <see cref="SearchCsProj"/>
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <returns></returns>
        private async Task<IEnumerable<IMagicPackage>> PopulatePackageReferences(string projectDirectory)
        {
            List<IMagicPackage> temp = new List<IMagicPackage>();
            string[] csProjFiles = Directory.GetFiles(projectDirectory, "*.csproj", SearchOption.AllDirectories);
            foreach (var csProjFile in csProjFiles)
            {
                temp.AddRange(await SearchCsProj(csProjFile));
            }
            return temp;
        }



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
