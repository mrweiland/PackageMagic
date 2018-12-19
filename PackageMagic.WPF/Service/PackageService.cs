using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using PackageMagic.WPF.Interface;
using PackageMagic.WPF.Model;

namespace PackageMagic.WPF.Service
{
    public class PackageService : IMagicPackageService
    {
        private List<IMagicPackage> _packages;

        public async Task<IEnumerable<IMagicPackage>> GetPackagesAsync(string aPath)
        {
            //Do the magic to get all kinds of packages
            //If this method includes any awaitable async call you can remove the 'await Task.Run' and make it to a normal method
            //This is only made to make a non async method to behave as an async method against the outer world
            _packages = new List<IMagicPackage>();

            _packages.AddRange(await GetNpmPackages(aPath));
            _packages.AddRange(await GetNugetPackages(aPath));

            return _packages;
        }

        private async Task<IEnumerable<NpmPackage>> GetNpmPackages(string aPath)
        {
            return null;
        }

        private async Task<IEnumerable<NugetPackage>> GetNugetPackages(string aPath) => await Task.Run(() =>
          {
              List<NugetPackage> result = new List<NugetPackage>();
              IEnumerable<string> csProjectFiles = Directory.EnumerateFiles(aPath, "*.csproj", SearchOption.AllDirectories);
              Parallel.ForEach(csProjectFiles, async (path) => result.AddRange(await GetInternalNuget(path)));
              Parallel.ForEach(csProjectFiles, async (path) => result.AddRange(await GetExternalNuget(path)));
              return result;
          });

        private async Task<IEnumerable<NugetPackage>> GetExternalNuget(string aPackageFile) => await Task.Run(() =>
        {
            List<ExternalNugetPackage> packages = new List<ExternalNugetPackage>();
            return packages;
        });

        private async Task<IEnumerable<NugetPackage>> GetInternalNuget(string aProjectFile) => await Task.Run(() =>
        {
            List<InternalNugetPackage> packages = new List<InternalNugetPackage>();

            using (FileStream fs = new FileStream(aProjectFile, FileMode.Open))
            {
                XDocument doc = XDocument.Load(fs);
                XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

                string targetFrameworkVersion = "";

                XElement targetFrameworkVersionElement = doc.Descendants("TargetFrameworkVersion").FirstOrDefault();
                if (targetFrameworkVersionElement == null)
                {
                    targetFrameworkVersionElement = doc.Descendants(msbuild + "TargetFrameworkVersion").FirstOrDefault();
                }

                targetFrameworkVersion = targetFrameworkVersionElement == null ? "" : targetFrameworkVersionElement.Value;

                IEnumerable<XElement> references = doc.Descendants("Reference");
                if (references.Count() == 0)
                {
                    references = doc.Descendants(msbuild + "Reference");
                }

                Parallel.ForEach(references, (reference) =>
                {
                    string packageName = reference.Attribute("Include") == null ? "" : reference.Attribute("Include").Value;
                    string packageVersion = reference.Attribute("Version") == null ? "" : reference.Attribute("Version").Value;
                    if (string.IsNullOrEmpty(packageVersion))
                    {
                        XElement versionElement = reference.Element("Version") ?? null;
                        if (versionElement != null)
                        {
                            packageVersion = versionElement.Value ?? "";
                        }
                    }

                    packageVersion = string.IsNullOrEmpty(packageVersion) ? targetFrameworkVersion : packageVersion;

                    InternalNugetPackage package = new InternalNugetPackage { Id = packageName, IncludedInProject = aProjectFile };
                    packages.Add(package);
                    //Callback?.Invoke(this, $"Found internal ref to {package.Id} {package.Version} in {Path.GetFileName(item.ProjectFile)}");
                });
            }
            return packages;
        });

        public IEnumerable<InternalNugetPackage> GetPackageReferences(string path)
        {
            List<InternalNugetPackage> informationItems = new List<InternalNugetPackage>();
            IEnumerable<string> csProjectFiles = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);
            Parallel.ForEach(csProjectFiles, new ParallelOptions { MaxDegreeOfParallelism = 8 }, (file) =>
            {
                InternalNugetPackage infoItem = new InternalNugetPackage { IncludedInProject = file };

                string packagesFile = Path.Combine(Path.GetDirectoryName(file), "packages.config");
                if (File.Exists(packagesFile))
                {
                    infoItem.PackagesFile = packagesFile;
                };
                lock (refLock)
                {
                    informationItems.Add(infoItem);
                }
                Callback?.Invoke(this, $"Added {Path.GetFileName(infoItem.ProjectFile)}");
            });
            //add more to search for
            return informationItems;
        }

        private IEnumerable<NugetPackage> GetNugetPackages(InformationItem item)
        {
            //TODO! Complete properties
            List<NugetPackage> packages = new List<NugetPackage>();

            if (!string.IsNullOrEmpty(item.PackagesFile))
            {
                PackageReferenceFile file = new PackageReferenceFile(item.PackagesFile);
                Parallel.ForEach(file.GetPackageReferences(), (packageReference) =>
                {
                    NugetPackage package = new NugetPackage { Id = packageReference.Id, Name = packageReference.Id, PackageType = PackageType.Nuget, Version = packageReference.Version.ToNormalizedString(), CsProjFile = item.ProjectFile, PackagesFile = item.PackagesFile };
                    lock (nugetLock)
                    {
                        packages.Add(package);
                    }
                    Callback?.Invoke(this, $"Found external nuget ref to {package.Id} {package.Version} in {Path.GetFileName(item.ProjectFile)}");
                });
            }
            packages.AddRange(GetIncludedNugetPackages(item));
            return packages;
        }


    }
}
