using g_aideUWP.DAO;// a changer pour la decoupe en couche !!!
using g_aideUWP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace g_aideUWP.ViewModel
{
    class ConnectionPageModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        private ICommand _connexionAppCommand;

        private UserConnection uc= new UserConnection();// a voir si c est ici comme ca
        private ServicesDAO services = new ServicesDAO();//same
        private ListServiceModel lsm;        

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

        private void ConnexionApp()
        {
            _navigationService.NavigateTo("ListService");
        }


    }
}
