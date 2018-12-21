using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackageMagic.Nuget;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
using PackageMagic.PackageService.Type;
using PackagesNpm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Service
{
    public class MagicProjectService : IMagicProjectService
    {
        public MessageDelegate MessageCallback { get; set; }

        public async Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch)
        {
            List<IMagicProject> listProjects = new List<IMagicProject>();

            string[] projectFiles = Directory.GetFiles(pathToSearch, "*.csproj", SearchOption.AllDirectories);

            string[] projectFilesNpm = Directory.GetFiles(pathToSearch, "package.json", SearchOption.AllDirectories);
            foreach (var packageJson in projectFilesNpm)
            {
                Console.WriteLine(packageJson);
                var project = new MagicProjectNpm { Name = Path.GetDirectoryName(packageJson), Path = packageJson };
                await project.Parse();
                IMagicPackageSearch theSearcher = new Npm();
                project.Packages.AddRange(await theSearcher.SearchPackages(project.Path));
                listProjects.Add(project);
            }
            foreach (var csProjFile in projectFiles)
            {
                MessageCallback?.Invoke($"Parsing {csProjFile}");

                var project = new MagicProjectCs { Name = Path.GetFileName(csProjFile), Path = csProjFile };
                await project.Parse();

                //TODO! Bad naming convention forces the use of full namespace
                //Consider using different namespace and class name for 'Nuget'
                IMagicPackageSearch theSearcher = new Nuget.Nuget();
                theSearcher.MessageCallback += MessageCallback;

                //Use the nuget object for parsing nuget package references
                project.Packages.AddRange(await theSearcher.SearchPackages(project.Path));
                theSearcher.MessageCallback -= MessageCallback;

                //TODO! Follow same naming conventions on namespaces and classnames in both Nuget and Npm library!
                //theSearcher = new Npm();
                //theSearcher.MessageCallback += MessageCallback;

                //Use the npm object for parsing npm package references
                //project.Packages.AddRange(await theSearcher.SearchPackages(project.Path));
                //theSearcher.MessageCallback -= MessageCallback;

                listProjects.Add(project);
            }


            return listProjects;
        }
    }
}
