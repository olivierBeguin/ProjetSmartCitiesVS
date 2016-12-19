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
        private INavigationService _navigationService;
        private Service _selectedService;
        private ICommand _listService;
        private ICommand _EditCommand;

        IEnumerable<Service> allServices;
        private UserConnection uc = new UserConnection();// a voir si c est ici comme ca
        private ServicesDAO services = new ServicesDAO();//same

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

        public ListServiceModel(INavigationService navigationService)
        {
            //GetAllServices();
            _navigationService = navigationService;

            if (IsInDesignMode)
            {
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
            var service = new ServicesDAO();
            string tokenAccess = await uc.GetToken();  // a mettre autre part et a retenir le token dans l app, vault ?
            var allServices = await service.GetServices(tokenAccess);
            Services = new ObservableCollection<Service>(allServices);
        }

        public async void GetAllServices()
        {
            string tokenAccess = await uc.GetToken();  // a mettre autre part et a retenir le token dans l app, vault ?
            allServices = await services.GetServices(tokenAccess);
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
