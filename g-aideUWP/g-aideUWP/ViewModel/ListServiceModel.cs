using g_aideUWP.DAO;
using g_aideUWP.Exceptions;
using g_aideUWP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Navigation;

namespace g_aideUWP.ViewModel
{
    public class ListServiceModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Service> _services = null;
        private string tokenAccess;

        public ObservableCollection<Service> Services
        {
            get { return _services; }
            set
            {
                if (_services == value)
                {
                    return;
                }
                _services = value;
                RaisePropertyChanged("Services");
            }
        }


        private ObservableCollection<CategoryService> _category;
        public ObservableCollection<CategoryService> ListCategory
        {
            get { return _category; }
            set
            {
                if (_category == value)
                {
                    return;
                }
                _category = value;
                RaisePropertyChanged("ListCategory");
            }
        }


        private IDialogService dialogService;

        private INavigationService _navigationService;
        ObservableCollection<Service> observableCollectionAllServices = new ObservableCollection<Service>();
        public ListServiceModel(INavigationService navigationService, IDialogService dialogService)
        {
            InitializeAsync();
            this.dialogService = dialogService;
            _navigationService = navigationService;
        }

        private ServicesDAO services = new ServicesDAO();
        public async Task InitializeAsync()
        {
            try
            {
                tokenAccess = GetTokenVault();
                var allCategory = await services.GetCategory(tokenAccess);
                var allServices = await services.GetServices(tokenAccess);
                
                ListCategory = new ObservableCollection<CategoryService>(allCategory);

                Services = new ObservableCollection<Service>(allServices);
            }
            catch(DataNotAvailableException e)
            {
                await dialogService.ShowMessage(e.GetMessage(),
                        "Erreur",
                        buttonConfirmText: "OK", buttonCancelText: "Annuler",
                        afterHideCallback: (confirmed) =>
                        {

                        });
            }
        }


        private Service _selectedService;
        public Service SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                if (_selectedService != null)
                {
                    RaisePropertyChanged("SelectedService");
                }
            }
        }

        
        private CategoryService _selectedCategory;
        public CategoryService SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                    _selectedCategory = value;

                    if (_selectedCategory != null)
                    {
                        RaisePropertyChanged("SelectedCategory");
                    }
            }
        }

        private ICommand _EditCommand;
        public ICommand EditCommand
        {
            get
            {
                if (this._EditCommand == null)
                {
                    _EditCommand = new RelayCommand(() => EditService());
                }
                return this._EditCommand;
            }
        }

        private ICommand _listService;
        public ICommand RemoveCommand
        {
            get
            {
                if (this._listService == null)
                {
                        _listService = new RelayCommand(() => RemoveServiceCommand());
                }
                return this._listService;
            }
        }


        private void EditService()
        {
            if (CanExecute())
            {
                _navigationService.NavigateTo("EditCategory", SelectedService);
            }
        }

        public bool CanExecute()
        {
            return (SelectedService != null);
        }

        private async void RemoveServiceCommand()
        {
            if (CanExecute())
            {
                try
                {
                    tokenAccess = GetTokenVault();
                    services.RemoveService(SelectedService, tokenAccess);
                    await InitializeAsync();
                    
                }
                catch(DataUpdateException e)
                {
                    await dialogService.ShowMessage(e.GetMessage(),
                            "Erreur",
                            buttonConfirmText: "OK", buttonCancelText: "Annuler",
                            afterHideCallback: (confirmed) =>
                                {

                                });
                }
                catch (DataNotAvailableException e)
                {
                    await dialogService.ShowMessage(e.GetMessage(),
                            "Erreur",
                            buttonConfirmText: "OK", buttonCancelText: "Annuler",
                            afterHideCallback: (confirmed) =>
                            {

                            });
                }
            }
        }

        public string GetTokenVault()
        {
            try
            {
                Windows.Security.Credentials.PasswordCredential credential = null;

                var vault = new Windows.Security.Credentials.PasswordVault();
                var storedCredential = vault.FindAllByResource("G-Aide");
                if (storedCredential.Count > 0)
                {
                    credential = vault.Retrieve("G-Aide", "MKTIG");
                }

                if (credential != null)
                {
                    return credential.Password;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
