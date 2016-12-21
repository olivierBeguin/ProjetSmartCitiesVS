// mettre que il n y a que l admin qui a acces a l uwp
using g_aideUWP.DAO;
using g_aideUWP.Exceptions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;

namespace g_aideUWP.ViewModel
{
    class ConnectionPageModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        
        private UserConnection uc = new UserConnection();// a voir si c est ici comme ca faut garder le token en memoire et le demander qu une seule fois 
        private IDialogService dialogService;    

        public ConnectionPageModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            _navigationService = navigationService;
        }

        private ICommand _connexionAppCommand;
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
            try
            {
                string tokenAccess = await uc.GetToken2();  // a mettre autre part et a retenir le token dans l app, vault ?
                //if (string.Equals(UserName,"admin")) // a mettre si on veut qu'il n'y ai que l'admin qui ait acces a cette application. et +1 exception du coup.
                //{
                    _navigationService.NavigateTo("ListService");
                //}
            }
            catch(ConnectionException e)
            {
                await dialogService.ShowMessage(e.GetMessage(),
                        "Erreur",
                        buttonConfirmText: "OK", buttonCancelText: "Annuler",
                        afterHideCallback: (confirmed) =>
                            {

                            });
            }
        }


        private string _userName;
        public string UserName // apres tout les test enlever la fonction que les champ sont pré remplis ( idem pour password)
        {
            get
            {
                var appData = Windows.Storage.ApplicationData.Current;
                var roamingSettings = appData.RoamingSettings;

                if (roamingSettings.Values.ContainsKey("UserName"))
                {
                    _userName = roamingSettings.Values["UserName"].ToString();
                }

                return _userName;
            }

            set
            {
                _userName = value;
                if (_userName != null)
                {
                    var appData = Windows.Storage.ApplicationData.Current;
                    var roamingSettings = appData.RoamingSettings;
                    roamingSettings.Values["UserName"] = _userName;

                    RaisePropertyChanged("UserName");
                }
            }
        }


        private string _password;
        public string Password
        {
            get
            {
                var appData = Windows.Storage.ApplicationData.Current;
                var roamingSettings = appData.RoamingSettings;

                if (roamingSettings.Values.ContainsKey("Password"))
                {
                    _password = roamingSettings.Values["Password"].ToString();
                }

                return _password;
            }

            set
            {
                _password = value;
                if (_password != null)
                {
                    var appData = Windows.Storage.ApplicationData.Current;
                    var roamingSettings = appData.RoamingSettings;
                    roamingSettings.Values["Password"] = _password;

                    RaisePropertyChanged("Password");
                }
            }
        }


    }
}
