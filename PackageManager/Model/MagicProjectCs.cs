using PackageMagic.General.Interface;
using PackageMagic.General.Type;
using PackageMagic.Nuget;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Type;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PackageMagic.PackageService.Model
{
    public class MagicProjectCs : MagicProjectBase
    {
        public MagicProjectCs(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
        }

        public override async Task ParseAsync() => await Task.Run(() =>
        {
            XDocument document = null;

            using (XmlTextReader tr = new XmlTextReader(Path))
            {
                tr.Namespaces = false;
                document = XDocument.Load(tr);
            }

            ProjectType = GetProjectType(document);

            if (ProjectType == FrameworkKind.DOTNET)
            {
                FrameworkVersion = GetFrameworkVersion(document);
                Packages = GetReferences(document);
                Packages.AddRange(GetNugetReferences(document));
            }
            else
            {
                FrameworkVersion = GetFrameworkVersionCore(document);
                Packages = GetReferences(document);
                Packages.AddRange(GetNugetReferencesCore(document));
            }
        });

        private FrameworkKind GetProjectType(XDocument document)
        {
            if (document.Root.Attribute("ToolsVersion") != null)
                return FrameworkKind.DOTNET;
            else
                return FrameworkKind.DOTNETCORE;
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
                result.Add(new BasicPackage { Name = reference.Attribute("Include").Value, Version = FrameworkVersion, PackageType = PackageKind.Reference });
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
                result.Add(new NugetPackage { Name = reference.Attribute("Include").Value, Version = version, PackageType = PackageKind.PackageReference });
            }
            return result;
        }

        private IEnumerable<IMagicPackage> GetNugetReferencesCore(XDocument document)
        {
            List<IMagicPackage> result = new List<IMagicPackage>();
            foreach (XElement reference in document.Descendants("PackageReference"))
            {
                result.Add(new NugetPackage { Name = reference.Attribute("Include").Value, Version = reference.Attribute("Version").Value, PackageType = PackageKind.PackageReference });
            }
            return result;
        }
    }
}
