using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PackageMagic.Nuget.interfaces;
using NuGet;
using PackageMagic.Nuget;
using PackageMagic.PackageManager;
using PackagesNpm;

namespace WindowsFormsApp2
{
    public delegate void DoLog(IPackageMagic package);

    public partial class Form1 : Form
    {
        public  DoLog LogPackage { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Runner().GetAwaiter().GetResult();
            Runner().ConfigureAwait(false);
            LogPackage += DoLogPackage;

        }
        public  void DoLogPackage(IPackageMagic package)
        {
            //if (txtLog.InvokeRequired)
            //{
            //    this.BeginInvoke(new Action<IPackageMagic>(DoLogPackage), new object[] { package.Name });
            //}
            //else
            //{
            //    txtLog.AppendText(package + Environment.NewLine);
            //}
            
        }
        private async Task Runner()
        {
            List<Task> t = new List<Task>();
            var _projectDirectory = @"c:\git\";
            clsPackages.Callback += CallbackFromPackage;
            t.Add(clsNuget.SearchForPackagesConfig(_projectDirectory));
            t.Add(clsNuget.PopulatePackageReferences(_projectDirectory));
            t.Add(clsNpm.LoopPackageJson(@"C:\git\package.json"));
            await Task.WhenAll(t);
            clsPackages.Callback -= CallbackFromPackage;
        }

        private  void CallbackFromPackage(IPackageMagic package)
        {
            if(package!=null && package is IPackageMagic)
            {
                //Debug.WriteLine(((NugetPackage)package).Name);
                LogPackage(package);

            }
            else
            {
                // d
            }

        }
    }
}
