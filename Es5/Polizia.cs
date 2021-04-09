using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Es5
{
    static class Polizia
    {
        static string _connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString;

        public static List<Agente> ElencoAgenti(bool conAree = false)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("Select * from Agenti", conn))
            {
                List<Agente> agenti = new List<Agente>();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Agente agente = new Agente((int)reader["IdAgente"], reader["Nome"].ToString(),
                        reader["Cognome"].ToString(), reader["CodiceFiscale"].ToString(),
                        (DateTime)reader["DataNascita"], (int)reader["AnniServizio"]);

                    agenti.Add(agente);

                    if (conAree)
                        RecuperaAreeAgente(agente);
                }

                return agenti;
            }
        }

        public static List<Agente> ElencoAgentiPerArea(string codiceArea)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT Agenti.* FROM Agenti INNER JOIN Associazioni ON " +
                "Agenti.IdAgente = Associazioni.IdAgente INNER JOIN Aree ON Associazioni.IdArea = Aree.IdArea " +
                "WHERE Aree.CodiceArea = @codiceArea", conn))
            {
                cmd.Parameters.AddWithValue("@codiceArea", codiceArea);

                List<Agente> agenti = new List<Agente>();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    agenti.Add(new Agente((int)reader["IdAgente"], reader["Nome"].ToString(),
                        reader["Cognome"].ToString(), reader["CodiceFiscale"].ToString(),
                        (DateTime)reader["DataNascita"], (int)reader["AnniServizio"]));

                return agenti;
            }
        }
        public static List<Agente> ElencoAgentiConAlmenoAnniDiServizio(int anniServizio)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Agenti WHERE AnniServizio >= @anniServizio", conn))
            {
                cmd.Parameters.AddWithValue("@anniServizio", anniServizio);

                List<Agente> agenti = new List<Agente>();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    agenti.Add(new Agente((int)reader["IdAgente"], reader["Nome"].ToString(),
                        reader["Cognome"].ToString(), reader["CodiceFiscale"].ToString(),
                        (DateTime)reader["DataNascita"], (int)reader["AnniServizio"]));

                return agenti;
            }
        }

        public static Agente InserisciAgente(string nome, string cognome, string codiceFiscale, DateTime dataNascita, int anniServizio)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter("Select * from Agenti", conn))
            {
                DataSet ds = new DataSet();

                da.Fill(ds, "Agenti");

                DataTable tableAgenti = ds.Tables["Agenti"];

                DataRow row = tableAgenti.Rows.Add(0, nome, cognome, codiceFiscale, dataNascita, anniServizio);

                new SqlCommandBuilder(da);  // crea i comandi per l'update del db

                conn.Open();
                da.Update(tableAgenti);
                SqlCommand cmd = new SqlCommand("Select @@identity", conn); // select identity deve essere eseguito all'interno della
                                                                            // stessa connessione in cui è stato fatto l'update della datatable
                                                                            // quindi la connessione va aperta esplicitamente
                int idAgente = (int)(decimal)cmd.ExecuteScalar();
                conn.Close();

                return new Agente(idAgente, nome, cognome, codiceFiscale, dataNascita, anniServizio);
            }
        }

        // extra non richiesto
        private static void RecuperaAreeAgente(Agente agente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT Aree.* FROM Aree INNER JOIN Associazioni ON Aree.IdArea = Associazioni.IdArea " +
                "WHERE Associazioni.IdAgente = @idAgente", conn))
            {
                cmd.Parameters.AddWithValue("@idAgente", agente.IdAgente);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    agente.Aree.Add(
                        new Area((int)reader["IdArea"],
                            reader["CodiceArea"].ToString(),
                            (bool)reader["AltoRischio"]));
            }
        }
    }
}
