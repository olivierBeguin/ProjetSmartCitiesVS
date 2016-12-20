using g_aideUWP.Model;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Navigation;

namespace g_aideUWP.ViewModel
{
    class EditCategoryModel
    {
        public Service SelectedService { get; set; }

        private INavigationService _navigationService;

        [PreferredConstructor]
        public EditCategoryModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedService = (Service)e.Parameter;
        }

    }
}
