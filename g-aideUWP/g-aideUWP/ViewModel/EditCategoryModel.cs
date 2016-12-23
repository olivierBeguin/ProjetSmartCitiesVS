using g_aideUWP.DAO;
using g_aideUWP.Exceptions;
using g_aideUWP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace g_aideUWP.ViewModel
{
    class EditCategoryModel : ViewModelBase
    {

        public Service SelectedService { get; set; }

        private INavigationService _navigationService;

        private IDialogService dialogService;

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
                RaisePropertyChanged("ListCategory");
            }
        }

        [PreferredConstructor]
        public EditCategoryModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            _navigationService = navigationService;
            InitializeAsync();
        }
            

        public async Task InitializeAsync()
        {
            try
            {
                string tokenAccess = tokenAccess = GetTokenVault();
                var allCategory = await services.GetCategory(tokenAccess);
                ListCategory = new ObservableCollection<CategoryService>(allCategory);
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

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedService = (Service)e.Parameter;
            SelectedCategory = SelectedService.Category;   
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

        private ICommand _categoryEdit;
        public ICommand EditCategoryCommand
        {
            get
            {
                if (this._categoryEdit == null)
                {
                    _categoryEdit = new RelayCommand(() => EditServiceCommand());
                }
                return this._categoryEdit;
            }
        }


        private ServicesDAO services = new ServicesDAO();
        private async void EditServiceCommand()
        {
            if (CanExecute())
            {
                try
                {
                    SelectedService.Category = SelectedCategory;
                    string tokenAccess = GetTokenVault();
                    services.EditService(SelectedService, tokenAccess);
                    _navigationService.NavigateTo("ListService");
                }
                catch (DataUpdateException e)
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

                public bool CanExecute()
        {
            return (SelectedService != null);
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
