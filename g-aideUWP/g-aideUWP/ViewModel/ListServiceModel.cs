using g_aideUWP.DAO;
using g_aideUWP.Exceptions;
using g_aideUWP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace g_aideUWP.ViewModel
{
    public class ListServiceModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Service> _services = null;

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


        private ObservableCollection<CategoryService> _category = null;
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
                RaisePropertyChanged("Category");
            }
        }


        private IDialogService dialogService;

        private INavigationService _navigationService;
        public ListServiceModel(INavigationService navigationService, IDialogService dialogService)
        {
            InitializeAsync();
            this.dialogService = dialogService;
            _navigationService = navigationService;
            

            //if (IsInDesignMode)
            //{
            //    var allCategory = new AllCategory();
            //    var category = new List<CategoryService>();

            //    foreach(CategoryService categoryService in category)
            //    {
            //        category.Add(new CategoryService());
            //    }
            //    allCategory.AllCategoryService = category;
            //    ListCategory = new ObservableCollection<CategoryService>(category);

            //    var allServices = new AllService();
            //    var services = new List<Service>();

            //    foreach (Service service in services)
            //    {
            //        services.Add(new Service());
            //    }
            //    allServices.AllServices = services;
            //    Services = new ObservableCollection<Service>(services);
            //}
            //else
            //{
                
            //}

        }


        private UserConnection uc = new UserConnection();// a voir si c est ici comme ca faut garder le token en memoire et le demander qu une seule fois
        private ServicesDAO services = new ServicesDAO();
        public async Task InitializeAsync()
        {
            try
            {
                string tokenAccess = await uc.GetToken2();  // a mettre autre part et a retenir le token dans l app, vault ? view model locator ? et remmetre gettoken()
                var category = await services.GetCategory(tokenAccess);
                var allServices = await services.GetServices(tokenAccess);
                ListCategory = new ObservableCollection<CategoryService>(category);
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
                    string tokenAccess = await uc.GetToken2();  // a mettre autre part et a retenir le token dans l app, vault ? et remttre gettoken
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

    }
}
