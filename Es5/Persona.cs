using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es5
{
    abstract class Persona
    {
        public string Nome { get; }
        public string Cognome { get; }
        public string CodiceFiscale { get; }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() && CodiceFiscale == ((Persona)obj).CodiceFiscale;
        }

        public override int GetHashCode()
        {
            return CodiceFiscale.GetHashCode();
        }

        public Persona(string nome, string cognome, string codiceFiscale)
        {
            Nome = nome;
            Cognome = cognome;
            CodiceFiscale = codiceFiscale;
        }
    }
}
