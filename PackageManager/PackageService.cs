using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageService.Interfaces
{
    public class MagicProjectService
    {
        public MagicProjectService()
        {
        }

        static async Task<IEnumerable<IMagicProject>> GetProjects(string pathToSearch)
        {
            List<IMagicProject> listProjects = new List<IMagicProject>();
            string[] projectFiles = Directory.GetFiles(pathToSearch, "*.csproj", SearchOption.AllDirectories);
            foreach (var csProjFile in projectFiles)
            {
                //listProjects.Add(new IMagicProject())
            }
            //var projects = await GetProjects();

            //foreach (var project in IMagicProject)
            //{ 
            //    // project.Packages.AddRange(await npm.SearchPackages(project.path))
            //    // project.Packages.AddRange(await  nuget.SearchPackages(project.path))
            //}
        }

        
    }
        public interface IMagicProject
    {
        //string Id { get; set; }
        string Name { get; set; }
        string Path { get; set; }
        List<IMagicPackage> Packages { get; set; }
    }

    public interface IMagicPackage
    {
        //string Id { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        IMagickPackageType PackageType { get; set; }
        
    }
    public interface IMagicPackageSearch
    {
        //string Id { get; set; }
        Task<IEnumerable<IMagicPackage>> SearchPackages(string path);
        Task AddPackages(IMagicPackage package);
        IList<IMagicPackage> GetPackages();
    }

    public enum IMagickPackageType
    {
        Npm,
        Nuget,
        PackageReference,
        Reference,
        PackageConfig
    }
    //public class PackageMagic
    //{
    //}
}
