using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace SondageSite.Models
{
    public class Option
    {
        public int IdSondage { get; private set; }
        public int IdOption { get; private set; }
        public string ReponseTxt { get; private set; }
        public int NbVotes { get; private set; }

        public Option(int id, int idSondage, string reponseTxt, int nbVotes)
        {
            IdOption = id;
            IdSondage = idSondage;
            ReponseTxt = reponseTxt;
            NbVotes = nbVotes;
        }

        public static void MettreAJourNbVotes(int idOption)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(@"UPDATE [Option] SET NbVotes = NbVotes + 1 WHERE IdOption = @IdOption", connection);
                command.Parameters.AddWithValue("@IdOption", idOption);
                command.ExecuteNonQuery();
            }
        }

        public static List<Option> ChargerOptionsDepuisBDD(int idSondage)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand recupererOption = connection.CreateCommand();
            recupererOption.CommandText = "SELECT * FROM [Option] WHERE IdSondage = @IdSondage";
            // Add parameter values
            recupererOption.Parameters.AddWithValue("@IdSondage", idSondage);
            SqlDataReader reader = recupererOption.ExecuteReader();

            List<Option> optionList = new List<Option>();

            while (reader.Read())
            {
                int id = (int)reader["IdOption"];
                string reponseTxt = (string)reader["ReponseTxt"];
                int nbVotes = (int)reader["NbVotes"];
                Option monOption = new Option(id, idSondage, reponseTxt, nbVotes);
                optionList.Add(monOption);
            }

            if (connection.State == ConnectionState.Open)
                connection.Close();

            return optionList;
        }

        public static void InsererOptionEnBdd(int idSondage, string reponseTxt)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand ajoutReponse = connection.CreateCommand();
            ajoutReponse.CommandText = "INSERT INTO [Option] (ReponseTxt, IdSondage, NbVotes) OUTPUT INSERTED.IdOption VALUES (@Reponse, @IdSondage, @NbVotes)";
            ajoutReponse.Parameters.AddWithValue("@Reponse", reponseTxt);
            ajoutReponse.Parameters.AddWithValue("@IdSondage", idSondage);
            ajoutReponse.Parameters.AddWithValue("@NbVotes", 0);
            // Get the inserted id
            int insertedID = (int)ajoutReponse.ExecuteScalar();
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

    }
}