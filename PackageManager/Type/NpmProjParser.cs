using Newtonsoft.Json.Linq;
using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackagesNpm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PackageMagic.PackageService.Type
{
    public class NpmProjParser
    {
        public string Name { get; set; }
        public List<IMagicPackage> Packages { get; set; }

        public static NpmProjParser Parse(string path)
        {
            NpmProjParser result = new NpmProjParser();
            dynamic jObject = JObject.Parse(File.ReadAllText(path));

            //TODO Fix so that Packages initalize an empty List
            result.Name = result.GetName(jObject);
            result.Packages = result.GetDevDependencies(jObject);
            result.Packages.AddRange(result.GetDependencies(jObject));




            //result.Packages.AddRange(tempNpmPackages);
            return result;
        }
        private IEnumerable<IMagicPackage> GetDevDependencies(dynamic jObject)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            IList<JToken> jsonDevDep = jObject["devDependencies"];
            if (jsonDevDep != null)
            {
                foreach (var jToken in jsonDevDep)
                {
                    var p = (JProperty)jToken;
                    result.Add(new BasicPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = PackageKind.Npm });
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
                    result.Add(new BasicPackage { Name = p.Name, Version = p.Value.ToString(), Description = "", PackageType = PackageKind.Npm });
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

