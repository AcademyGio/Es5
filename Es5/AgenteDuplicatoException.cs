using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es5
{
    // custom exception
    class AgenteDuplicatoException : Exception
    {
        // costruttore di default di solito c'è
        public AgenteDuplicatoException()
        {
        }

        // costruttore per inizializzare gli eventuali membri specifici
        public AgenteDuplicatoException(string codiceFiscale)
        {
            CodiceFiscale = codiceFiscale;
        }

        // altri costruttori che richiamano quelli della classe base
        public AgenteDuplicatoException(string codiceFiscale, string message)
            : base(message)
        {
            CodiceFiscale = codiceFiscale;
        }

        // membri specifici per la custom exception
        public string CodiceFiscale { get;}
    }
}
