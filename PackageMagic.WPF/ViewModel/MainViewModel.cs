using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PackageMagic.WPF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PackageMagic.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //This is our data service injected from the ViewModelLocator
        private IMagicPackageService _packageService;

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

        //This property is keeping track if we are running a refresh for the moment, it toggles the Refresh button on and off
        public bool Busy
        {
            get => _busy;
            set => Set(() => Busy, ref _busy, value);
        }
        private bool _busy;

        //This is the binding for the button Refresh on the toolbar, 
        //the first parameter is an async lambda for Execute and the second is a lambda for CanExecute
        public RelayCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(
                       async () => { await Refresh(); },
                       () => { return !Busy; }));
        private RelayCommand _refreshCommand;

        //This async task will be executed when the user pushes Refresh
        private async Task Refresh()
        {
            Busy = true;
            RefreshCommand.RaiseCanExecuteChanged();
            Status = "Refreshing";
            _packageService.MyCallback += UpdateStatusExternal;
            Packages = await _packageService.GetPackagesAsync(@"C:\Repos");

            //Just as POC I will delay the task here to see if everything gets updated accordingly and that the application don't freeze
            await Task.Delay(2000);

            Status = "";
            Busy = false;
            RefreshCommand.RaiseCanExecuteChanged();
        }

        private void UpdateStatusExternal(string status) => Status = status;

        //This task property keeps track if the async Initialize has run to completion
        public Task Initialized
        {
            get => _initialized;
            private set => _initialized = value;
        }
        private Task _initialized;

        public MainViewModel(IMagicPackageService packageService)
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
            await Refresh();

            if (IsInDesignMode)
            {
                SelectedPackage = Packages.FirstOrDefault();
            }
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
