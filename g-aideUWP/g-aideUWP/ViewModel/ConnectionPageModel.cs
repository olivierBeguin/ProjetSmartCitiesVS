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
        
        private UserConnection uc = new UserConnection();
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
                string tokenAccess = await uc.GetToken(UserName,Password);

                if (string.Equals(UserName,"admin") || string.Equals(UserName,"louvdd@hotmail.com"))
                {
                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Add(new Windows.Security.Credentials.PasswordCredential(
                    "G-Aide", "MKTIG" , tokenAccess));
                
                _navigationService.NavigateTo("ListService");
                }
                else
                {
                    await dialogService.ShowMessage("Ce compte n'est pas un compte administrateur. Seul l'administrateur a acces à cette application.",
                        "Erreur",
                        buttonConfirmText: "OK", buttonCancelText: "Annuler",
                        afterHideCallback: (confirmed) =>
                        {

                        });
                }
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
