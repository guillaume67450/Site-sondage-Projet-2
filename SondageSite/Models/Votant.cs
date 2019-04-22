using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace SondageSite.Models
{
    public class Votant
    {
        public int IdVotant { get; private set; }
        public string MomentVote { get; private set; }

        public static int InsererVotantEnBdd()
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand ajoutVotant = connection.CreateCommand();
            ajoutVotant.CommandText = "INSERT INTO Votant (MomentVote) OUTPUT INSERTED.IdVotant VALUES (GETDATE())";
            // Get the inserted id
            int insertedID = (int)ajoutVotant.ExecuteScalar();
            if (connection.State == ConnectionState.Open)
                connection.Close();
            // Debug.Write("ok");
            return insertedID;
        }
    }
}