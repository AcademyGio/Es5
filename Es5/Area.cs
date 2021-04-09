using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es5
{
    class Area
    {
        public int IdArea { get; }
        public string CodiceArea { get; }
        public bool AltoRischio { get; }

        public Area(int idArea, string codiceArea, bool altoRischio)
        {
            IdArea = idArea;
            CodiceArea = codiceArea;
            AltoRischio = altoRischio;
        }

        public List<Agente> Agenti { get; } = new List<Agente>();
    }
}
