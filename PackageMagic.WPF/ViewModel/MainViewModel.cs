using GalaSoft.MvvmLight;
using PackageMagic.WPF.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageMagic.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //This is our data service injected from the ViewModelLocator
        private IPackageService _packageService;

        //This should be bound to the controls in the PackageListView
        public IEnumerable<IMagicPackage> Packages
        {
            get => _packages;
            set => Set(() => Packages, ref _packages, value);
        }
        private IEnumerable<IMagicPackage> _packages;

        //This should be bound to the controls in the PackageItemView
        public IMagicPackage SelectedPackage
        {
            get => _selectedPackage;
            set => Set(() => SelectedPackage, ref _selectedPackage, value);
        }
        private IMagicPackage _selectedPackage;

        //This should be bound to a control in the StatusbarView
        public string Status
        {
            get => _status;
            set => Set(() => Status, ref _status, value);
        }
        private string _status;
        
        //This task property keeps track if the async Initialize has run to completion
        public Task Initialized {
            get => _initialized;
            private set
            {
                _initialized = value;
                Status = _initialized.Status.ToString();
            }
        }
        private Task _initialized;

        public MainViewModel(IPackageService packageService)
        {
            //This will be automatically injected by the ViewModelLocator
            _packageService = packageService;

            //This call will run async but can not be awaited in the constructor since it's not async
            //Instead it will add its task to the property Initialized that could be awaited somewhere else
            Initialized = Initialize();
        }

        private async Task Initialize()
        {
            //This will run async at startup and the property Initialized will be awaitable if needed.
            //You could even make something happen in the setter for the property to indicate that Initialize has been finished.
            await Task.Delay(2000);
            Packages = await _packageService.GetPackagesAsync(@"C:\Repos");
        }
    }
}

// To make the application Blendify you could use this to feed the views with sample data
//if (IsInDesignMode)
//{
//    // Code runs in Blend --> create design time data.
//}
//else
//{
//    // Code runs "for real"
//}
