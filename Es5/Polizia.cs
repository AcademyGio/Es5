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

        public static Agente RecuperaAgente(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("Select * from Agenti where IdAgente = @idAgente", conn))
            {
                cmd.Parameters.AddWithValue("@idAgente", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())  // se l'ho trovato
                    return new Agente((int)reader["IdAgente"], reader["Nome"].ToString(),
                        reader["Cognome"].ToString(), reader["CodiceFiscale"].ToString(),
                        (DateTime)reader["DataNascita"], (int)reader["AnniServizio"]);

                throw new AgenteNonTrovatoException();
            }
        }

        public static List<Agente> ElencoAgenti()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("Select * from Agenti", conn))
            {
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
                try
                {
                    da.Update(tableAgenti);
                }
                catch (SqlException ex)
                {
                    // indago ex per capire se è un'eccezione dovuta ad 
                    // agente duplicato (con stesso CF di un altro)
                    // nel caso genero un'eccezione di tipo AgenteDuplicato

                    if (ex.Message.Contains("IX_CodiceFiscale"))
                        throw new AgenteDuplicatoException(
                            "Non possono esistere agenti con codice fiscale uguale",
                            ex, // inner exception
                            codiceFiscale);
                    else if (ex.Message.Contains("CK_Maggiorenne"))
                        throw new AgenteMinorenneException(
                            "Non possono esistere agenti minorenni",
                            ex); // inner exception
                    else
                        throw;  // ciò che non conosco lo rimando al chiamante
                                // sperando che lo sappia gestire in qualche modo
                }

                int idAgente = 0;
                using (SqlCommand cmd = new SqlCommand("Select @@identity", conn))  // select identity deve essere eseguito all'interno della
                                                                                    // stessa connessione in cui è stato fatto l'update della datatable
                                                                                    // quindi la connessione va aperta esplicitamente
                {
                    idAgente = (int)(decimal)cmd.ExecuteScalar();
                }

                conn.Close();

                return new Agente(idAgente, nome, cognome, codiceFiscale, dataNascita, anniServizio);
            }
        }
    }
}
