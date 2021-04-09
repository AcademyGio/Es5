using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es5
{
    class Agente : Persona
    {
        public int IdAgente { get; }
        public DateTime DataNascita { get; }
        public int AnniServizio { get; }

        public Agente(int idAgente, string nome, string cognome, string codiceFiscale, DateTime dataNascita, int anniServizio)
            : base(nome, cognome, codiceFiscale)
        {
            IdAgente = idAgente;
            DataNascita = dataNascita;
            AnniServizio = anniServizio;
        }

        public override string ToString()
        {
            return $"{CodiceFiscale} - {Nome} {Cognome} - {AnniServizio} anni di servizio";
        }

        public List<Area> Aree { get; } = new List<Area>();
    }
}
