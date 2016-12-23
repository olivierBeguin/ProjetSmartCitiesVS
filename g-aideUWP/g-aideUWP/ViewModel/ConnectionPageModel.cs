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
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
                if (_userName != null)
                {
                    RaisePropertyChanged("UserName");
                }
            }
        }


        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                if (_password != null)
                {
                    RaisePropertyChanged("Password");
                }
            }
        }


    }
}
