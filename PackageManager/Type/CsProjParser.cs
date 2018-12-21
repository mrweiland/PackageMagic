using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using PackageMagic.General.Interface;
using PackageMagic.General.Type;

namespace PackageMagic.PackageService.Type
{

    public class CsProjParser
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FrameworkVersion { get; set; }
        public ProjectType ProjectType { get; set; }
        public List<IMagicPackage> Packages { get; set; }

        public static CsProjParser Parse(string path)
        {
            XDocument document = null;

            using (XmlTextReader tr = new XmlTextReader(path))
            {
                tr.Namespaces = false;
                document = XDocument.Load(tr);
            }

            CsProjParser result = new CsProjParser();
            result.Name = System.IO.Path.GetFileName(path);
            result.Path = path;

            result.ProjectType = result.GetProjectType(document);

            if (result.ProjectType == ProjectType.DOTNET)
            {
                result.FrameworkVersion = result.GetFrameworkVersion(document);
                result.Packages = result.GetReferences(document);
                result.Packages.AddRange(result.GetNugetReferences(document));
            }
            else
            {
                result.FrameworkVersion = result.GetFrameworkVersionCore(document);
                result.Packages = result.GetReferences(document);
                result.Packages.AddRange(result.GetNugetReferencesCore(document));
            }

            return result;
        }

        private ProjectType GetProjectType(XDocument document)
        {
            if (document.Root.Attribute("ToolsVersion") != null)
                return ProjectType.DOTNET;
            else
                return ProjectType.DOTNETCORE;
        }

        private string GetFrameworkVersion(XDocument document)
        {
            XElement FrameworkVersionElement = document.Descendants("TargetFrameworkVersion").FirstOrDefault();
            return FrameworkVersionElement == null ? "" : FrameworkVersionElement.Value;
        }

        private string GetFrameworkVersionCore(XDocument document)
        {
            XElement FrameworkVersionElement = document.Descendants("TargetFramework").FirstOrDefault();
            return FrameworkVersionElement == null ? "" : FrameworkVersionElement.Value;
        }

        private List<IMagicPackage> GetReferences(XDocument document)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            foreach (XElement reference in document.Descendants("Reference"))
            {
                result.Add(new BasicPackage { Name = reference.Attribute("Include").Value, Version = FrameworkVersion, PackageType = MagicPackageType.Reference });
            }
            return result;
        }

        private IEnumerable<IMagicPackage> GetNugetReferences(XDocument document)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            foreach (XElement reference in document.Descendants("PackageReference"))
            {
                XElement versionNode = reference.Descendants("Version").FirstOrDefault();
                string version = versionNode == null ? "" : versionNode.Value;
                result.Add(new BasicPackage { Name = reference.Attribute("Include").Value, Version = version, PackageType = MagicPackageType.PackageReference });
            }
            return result;
        }

        private IEnumerable<IMagicPackage> GetNugetReferencesCore(XDocument document)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            foreach (XElement reference in document.Descendants("PackageReference"))
            {
                result.Add(new BasicPackage { Name = reference.Attribute("Include").Value, Version = reference.Attribute("Version").Value, PackageType = MagicPackageType.PackageReference });
            }
            return result;
        }
    }
}
