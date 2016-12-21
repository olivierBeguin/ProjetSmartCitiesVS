using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g_aideUWP.Exceptions
{
    class DataNotAvailableException : Exception
    {
        public string MessageError { get; set; }

        public DataNotAvailableException()
        {
            MessageError = "Impossible d'obtenir les données";
        }

        public string GetMessage()
        {
            return MessageError;
        }
    }
}
