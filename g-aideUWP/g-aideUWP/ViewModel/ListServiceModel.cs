using g_aideUWP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace g_aideUWP.ViewModel
{
    class ListServiceModel : ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<Service> _services;
        private INavigationService _navigationService;
        private Service _selectedService;
        private ICommand _editCategoryCommand;

        public ObservableCollection<Service> Services
        {
            get { return _services; }
            set
            {
                _services = value;
                RaisePropertyChanged("Services");
            }
        }

        public ListServiceModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
           Services = new ObservableCollection<Service>(AllService.GetAllStudents());
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

        public ICommand EditCategoryCommand
        {
            get
            {
                if (this._editCategoryCommand == null)
                {
                    this._editCategoryCommand = new RelayCommand(() => EditService());
                }
                return this._editCategoryCommand;
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


    }
}
