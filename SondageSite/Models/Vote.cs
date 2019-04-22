using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace SondageSite.Models
{
    public class Vote
    {
        public int IdVotant { get; private set; }
        public int IdOption { get; private set; }

        public Vote(int idVotant, int idOption)
        {
            IdVotant = idVotant;
            IdOption = idOption;
        }

        public static void InsererVoteEnBdd(Vote vote)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand ajoutVote = connection.CreateCommand();
            ajoutVote.CommandText = "INSERT INTO Vote (IdVotant, IdOption) OUTPUT INSERTED.IdVote VALUES (@IdVotant,@IdOption)";
            // Add parameter values
            ajoutVote.Parameters.AddWithValue("@IdVotant", vote.IdVotant);
            ajoutVote.Parameters.AddWithValue("@IdOption", vote.IdOption);
            // Get the inserted id
            //int insertedID = (int)ajoutVote.ExecuteScalar();
            if (connection.State == ConnectionState.Open)
                connection.Close();
            // Debug.Write("ok");
            //return insertedID;
        }
    }
}