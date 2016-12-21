using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Exceptions
{
    class DataUpdateException : Exception
    {
        public string MessageError { get; set; }

        public DataUpdateException()
        {
            MessageError = "Impossible de mettre à jour la base de donnée. Réessayez plus tard.";
        }

        public string GetMessage()
        {
            return MessageError;
        }
    }
}
