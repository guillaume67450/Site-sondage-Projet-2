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

        public static List<Option> ChargerOptionsDepuisBDD(int idSondage) // charger les questions avec idSondage
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand recupererOption = connection.CreateCommand();
            recupererOption.CommandText = "SELECT * FROM [Option] WHERE IdSondage = @IdSondage";
            // Add parameter values
            recupererOption.Parameters.AddWithValue("@IdSondage", idSondage);
            SqlDataReader reader = recupererOption.ExecuteReader(); //ExecuteReader est utilisé pour récupérer un jeu d'enregistrement et retourne un objet DataReader 

            List<Option> optionList = new List<Option>(); // on a ainsi notre liste de questions comme objet prêt à accueillir les valeurs

            while (reader.Read()) // tant qu'on peut avancer dans le jeu d'enregistrements, on affecte les valeurs suivantes aux variables grâce au jeu d'enregistrement 
            {
                int id = (int)reader["IdOption"];
                string reponseTxt = (string)reader["ReponseTxt"];
                int nbVotes = (int)reader["NbVotes"];
                Option monOption = new Option(id, idSondage, reponseTxt, nbVotes); // monOption prend les valeurs stockées dans les variables créées
                optionList.Add(monOption); //on ajoute à monOption
            }

            if (connection.State == ConnectionState.Open)
                connection.Close(); // si la connextion est encore ouverte, on la referme

            return optionList; // on retourne la liste des questions correspondant à IdSondage (avec leur IdOption, ReponseTxt et NbVotes et une Id propre)
        }

        public static void InsererOptionEnBdd(int idSondage, string reponseTxt)
        {  // déclaration d'InsererOptionEnBdd comme membre statique de la classe Option, ainsi les using SondageSite.Models peuvent utiliser Option.InsererOptionEnBdd avec ses paramètres
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand ajoutReponse = connection.CreateCommand();
            ajoutReponse.CommandText = "INSERT INTO [Option] (ReponseTxt, IdSondage, NbVotes) OUTPUT INSERTED.IdOption VALUES (@Reponse, @IdSondage, @NbVotes)";
            ajoutReponse.Parameters.AddWithValue("@Reponse", reponseTxt);
            ajoutReponse.Parameters.AddWithValue("@IdSondage", idSondage);
            ajoutReponse.Parameters.AddWithValue("@NbVotes", 0); // on initialise le nombre de votes à 0
            // Get the inserted id
            int insertedID = (int)ajoutReponse.ExecuteScalar(); //ExecuteScalar car récupère une valeur unitaire;
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

    }
}