using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackageMagic.Nuget;
using PackageMagic.ProjectService.Interface;
using PackageMagic.ProjectService.Model;
using PackageMagic.ProjectService.Type;
using PackagesNpm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.ProjectService.Service
{
    public class MagicProjectService : IMagicProjectService
    {
        public MessageDelegate MessageCallback { get; set; }

        public async Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch, ProjectKind projectKind)
        {
            List<IMagicProject> listProjects = new List<IMagicProject>();

            if (projectKind == ProjectKind.CSharp)
            {
                listProjects.AddRange(await PopulateWithCSharpProjects(pathToSearch));
            }

            if (projectKind == ProjectKind.VisualBasic)
            {
            }

            if (projectKind == ProjectKind.Npm)
            {
                listProjects.AddRange(await PopulateWithNpmProjects(pathToSearch));
            }

            return listProjects;
        }

        private async Task<IEnumerable<IMagicProject>> PopulateWithCSharpProjects(string pathToSearch)
        {
            List<IMagicProject> result = new List<IMagicProject>();

            string[] projectFiles = Directory.GetFiles(pathToSearch, "*.csproj", SearchOption.AllDirectories);
            foreach (var csProjFile in projectFiles)
            {
                MessageCallback?.Invoke($"Parsing {csProjFile}");

                var project = new MagicProjectCs(csProjFile);
                await project.ParseAsync();

                //TODO! Bad naming convention forces the use of full namespace
                //TODO! Don't do anything about this, I will fix so we have same pattern for all package types. /Lennart
                //TODO! It should work temporary as it is right now
                //but consider using different namespace and class name for 'Nuget' and refactor to same as MagicProjectCs and MagicProjectNpm 
                IMagicPackageSearch theSearcher = new Nuget.Nuget();
                theSearcher.MessageCallback += MessageCallback;
                //Use the nuget object for parsing nuget package references from packages.config
                project.Packages.AddRange(await theSearcher.SearchPackages(project.Path));
                theSearcher.MessageCallback -= MessageCallback;
                result.Add(project);
            }

            return result;
        }

        private async Task<IEnumerable<IMagicProject>> PopulateWithNpmProjects(string pathToSearch)
        {
            List<IMagicProject> result = new List<IMagicProject>();

            string[] projectFilesNpm = Directory.GetFiles(pathToSearch, "package.json", SearchOption.AllDirectories);
            foreach (var packageJson in projectFilesNpm)
            {
                MessageCallback?.Invoke($"Parsing {packageJson}");

                var project = new MagicProjectNpm(packageJson);
                await project.ParseAsync();
            }

            return result;
        }
    }
}
