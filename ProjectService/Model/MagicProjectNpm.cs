using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackagesNpm;

namespace PackageMagic.ProjectService.Model
{
    public class MagicProjectNpm : MagicProjectBase
    {
        public MagicProjectNpm(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
        }

        public override async Task ParseAsync() => await Task.Run(() =>
        {
            dynamic jObject = JObject.Parse(File.ReadAllText(Path));

            //TODO Fix so that Packages initalize an empty List
            Name = GetName(jObject);
            Packages = GetDevDependencies(jObject);
            Packages.AddRange(GetDependencies(jObject));

            //Was this the old code?
            //IMagicPackageSearch theSearcher = new Npm();
            //Packages.AddRange(await theSearcher.SearchPackages(project.Path));

        });

        private List<IMagicPackage> GetDevDependencies(dynamic jObject)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            IList<JToken> jsonDevDep = jObject["devDependencies"];

            if (jsonDevDep != null)
            {
                foreach (var jToken in jsonDevDep)
                {
                    var p = (JProperty)jToken;
                    result.Add(new NpmPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = PackageKind.Npm });
                }
            }

            return result;
        }

        private IEnumerable<IMagicPackage> GetDependencies(dynamic jObject)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            IList<JToken> jsonDevDep = jObject["dependencies"];

            if (jsonDevDep != null)
            {
                foreach (var jToken in jsonDevDep)
                {
                    var p = (JProperty)jToken;
                    result.Add(new NpmPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = PackageKind.Npm });
                }
            }

            return result;
        }

        private string GetName(dynamic jObject)
        {
            return jObject["name"].ToString();
        }
    }
}
