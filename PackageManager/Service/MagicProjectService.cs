using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Model;
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

        public async Task<IEnumerable<IMagicProject>> GetProjectsAsync(string pathToSearch) => await Task.Run(() =>
        {
            List<IMagicProject> listProjects = new List<IMagicProject>();

            string[] projectFiles = Directory.GetFiles(pathToSearch, "*.csproj", SearchOption.AllDirectories);
            foreach (var csProjFile in projectFiles)
            {
                MessageCallback?.Invoke($"Parsing {csProjFile}");
                var project = new MagicProjectCs { Name = Path.GetFileName(csProjFile), Path = csProjFile };
                //Create a nuget object for parsing nuget package references
                //Create a npm object for parsing npm package references
                //project.AddRange(await nuget.SearchPackages(project.Path));
                //project.AddRange(await npm.SearchPackages(project.Path));

                listProjects.Add(project);
            }

            return listProjects;
        });
    }
}
