/*
  In App.xaml is added:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:PackageMagic.WPF"
                           x:Key="Locator" />
  </Application.Resources>
  This will make the ViewModelLocator available for the whole applicattion

  In the Views xaml main element you add:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
  This will bind the viewmodel to the view and automatically inject any dependencies that are declared in the viewmodels constructor
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using PackageMagic.PackageService.Interface;
using PackageMagic.PackageService.Service;
using PackageMagic.WPF.Design;
using PackageMagic.WPF.Interface;
using PackageMagic.WPF.Service;

namespace PackageMagic.WPF.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IMagicProjectService, DesignMagicProjectService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IMagicProjectService, MagicProjectService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}