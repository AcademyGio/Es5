using System;

namespace Es5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benvenuto nel database agenti.");

            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Mostra tutti gli agenti");
                Console.WriteLine("2. Mostra gli agenti di un'area");
                Console.WriteLine("3. Mostra agenti con anni di servizio");
                Console.WriteLine("4. Inserisci nuovo agente");
                Console.WriteLine("0. Esci");

                switch (Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        MostraTuttiGliAgenti();
                        break;
                    case '2':
                        Console.WriteLine();
                        MostraAgentiArea();
                        break;
                    case '3':
                        Console.WriteLine();
                        MostraAgentiServizio();
                        break;
                    case '4':
                        Console.WriteLine();
                        InserisciAgente();
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("Scelta non valida");
                        break;
                }
            } while (true);
        }

        private static void InserisciAgente()
        {
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Cognome: ");
            string cognome = Console.ReadLine();
            Console.Write("Codice fiscale: ");
            string codiceFiscale = Console.ReadLine();

            DateTime dataNascita;
            do
            {
                Console.Write("Data di nascita: ");
            }
            while (!DateTime.TryParse(Console.ReadLine(), out dataNascita));

            int anniServizio;
            do
            {
                Console.Write("Anni di servizio: ");
            }
            while (!int.TryParse(Console.ReadLine(), out anniServizio));

            Agente agente = Polizia.InserisciAgente(nome, cognome, codiceFiscale, dataNascita, anniServizio);
            Console.WriteLine($"Inserito agente: {agente}");
        }

        private static void MostraAgentiServizio()
        {
            Console.Write("Agenti con anni di servizio maggiori o uguali a: ");
            int.TryParse(Console.ReadLine(), out int anniServizio);

            foreach (Agente a in Polizia.ElencoAgentiConAlmenoAnniDiServizio(anniServizio))
                Console.WriteLine(a);
        }

        private static void MostraAgentiArea()
        {
            Console.Write("Di quale area vuoi mostrare gli agenti? ");
            string codiceArea = Console.ReadLine();

            foreach (Agente a in Polizia.ElencoAgentiPerArea(codiceArea))
                Console.WriteLine(a);
        }

        private static void MostraTuttiGliAgenti()
        {
            foreach (Agente a in Polizia.ElencoAgenti(true))
                Console.WriteLine(a);
        }
    }
}
