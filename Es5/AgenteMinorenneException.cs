using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es5
{
    class AgenteMinorenneException : Exception
    {
        // costruttore di default di solito c'è
        public AgenteMinorenneException()
        {
        }

        // costruttore per inizializzare gli eventuali membri specifici
        public AgenteMinorenneException(string message)
            : base(message)
        {
        }

        public AgenteMinorenneException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
