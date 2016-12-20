using g_aideUWP.DAO;
using g_aideUWP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace g_aideUWP.ViewModel
{
    public class ListServiceModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Service> _services = null;
        private ObservableCollection<CategoryService> _category = null;
        private INavigationService _navigationService;
        private Service _selectedService;
        private ICommand _listService;
        private ICommand _EditCommand;

        IEnumerable<Service> allServices;
        private UserConnection uc = new UserConnection();// a voir si c est ici comme ca faut garder le token en memoire et le demander qu une seule fois
        private ServicesDAO services = new ServicesDAO();

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



        public ListServiceModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            if (IsInDesignMode)
            {
                var allCategory = new AllCategory();
                var category = new List<CategoryService>();

                foreach(CategoryService categoryService in category)
                {
                    category.Add(new CategoryService());
                }
                allCategory.AllCategoryService = category;
                ListCategory = new ObservableCollection<CategoryService>(category);


                var allServices = new AllService();
                var services = new List<Service>();

                foreach (Service service in services)
                {
                    services.Add(new Service());
                }
                allServices.AllServices = services;
                Services = new ObservableCollection<Service>(services);
            }
            else
            {
                InitializeAsync();
            }

        }

        public async Task InitializeAsync()
        {
            string tokenAccess = await uc.GetToken();  // a mettre autre part et a retenir le token dans l app, vault ?
            var category = await services.GetCategory(tokenAccess);
            var allServices = await services.GetServices(tokenAccess);
            ListCategory = new ObservableCollection<CategoryService>(category);
            Services = new ObservableCollection<Service>(allServices);
        }

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
                string tokenAccess = await uc.GetToken();  // a mettre autre part et a retenir le token dans l app, vault ?
                services.RemoveService(SelectedService, tokenAccess);
                _navigationService.NavigateTo("ListService");
            }
        }

    }
}
