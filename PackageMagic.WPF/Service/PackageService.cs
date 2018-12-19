using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet;
using PackageMagic.WPF.Interface;
using PackageMagic.WPF.Model;

namespace PackageMagic.WPF.Service
{

    public class PackageService : IMagicPackageService
    {
        private List<IMagicPackage> _packages;

        public StatusCallback MyCallback { get; set; }

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
            return new List<NpmPackage>();
        }

        private async Task<IEnumerable<NugetPackage>> GetNugetPackages(string aPath) => await Task.Run(() =>
          {
              List<NugetPackage> result = new List<NugetPackage>();
              IEnumerable<string> csProjectFiles = Directory.EnumerateFiles(aPath, "*.csproj", SearchOption.AllDirectories);
              Parallel.ForEach(csProjectFiles, async (path) => result.AddRange(await GetInternalNuget(path)));
              Parallel.ForEach(csProjectFiles, async (path) => result.AddRange(await GetExternalNuget(path)));
              return result;
          });

        private async Task<IEnumerable<NugetPackage>> GetExternalNuget(string aProjectFile) => await Task.Run(() =>
        {
            List<ExternalNugetPackage> packages = new List<ExternalNugetPackage>();

            var aPackageFile = Path.Combine(Path.GetDirectoryName(aProjectFile), "packages.config");

            MyCallback?.Invoke($"Exploring {aPackageFile}");

            if (File.Exists(aPackageFile))
            {
                PackageReferenceFile file = new PackageReferenceFile(aPackageFile);
                foreach(var packageReference in file.GetPackageReferences())
                {
                    packages.Add(new ExternalNugetPackage { Id = packageReference.Id, IncludedInProject = aProjectFile });
                }
            }

            return packages;
        });

        private async Task<IEnumerable<NugetPackage>> GetInternalNuget(string aProjectFile) => await Task.Run(() =>
        {
            List<InternalNugetPackage> packages = new List<InternalNugetPackage>();

            MyCallback?.Invoke($"Exploring {aProjectFile}");

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

                    packages.Add(new InternalNugetPackage { Id = packageName, IncludedInProject = aProjectFile });
                });
            }

            return packages;
        });

    }
}
