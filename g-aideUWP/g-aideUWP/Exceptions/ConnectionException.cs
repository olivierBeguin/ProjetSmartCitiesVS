using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Exceptions
{
    class ConnectionException : Exception
    {
        private bool ErrorEntry { get; set; }
        private string MessageError { get; set; }

        public ConnectionException(bool ErrorEntry)
        {
            this.ErrorEntry = ErrorEntry;
        }

        public ConnectionException(string message)
        {
            this.MessageError = MessageError;
        }

        public string GetMessage()
        {
            if(!ErrorEntry)
            {
                return "La connexion avec la base de données a un problème. Veuillez Réessayer plus tard.";
            }
            else
            {
                if(string.IsNullOrEmpty(MessageError))
                {
                    return "Vos identifiants sont incorrects";
                }
                else
                {
                    return MessageError;
                }
            }
        }
    }
}
