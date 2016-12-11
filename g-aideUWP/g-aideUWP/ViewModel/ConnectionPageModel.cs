using g_aideUWP.DAO;// a changer pour la decoupe en couche !!!
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace g_aideUWP.ViewModel
{
    class ConnectionPageModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        private ICommand _connexionAppCommand;
        private UserConnection uc= new UserConnection();// a voir si c est ici comme ca
        private ServicesDAO augu = new ServicesDAO();//same

        public ConnectionPageModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ICommand ConnexionAppCommand
        {
            get
            {
                if(this._connexionAppCommand == null)
                {
                    this._connexionAppCommand = new RelayCommand(() => ConnexionApp());
                }
                return this._connexionAppCommand;
            }
        }

        private async void ConnexionApp()
        {
            string tokenAccess = await uc.GetToken();  // a mettre autre part
            IEnumerable coucou = await augu.GetServices(tokenAccess);   //same
            _navigationService.NavigateTo("ListService");
        }
    }
}
