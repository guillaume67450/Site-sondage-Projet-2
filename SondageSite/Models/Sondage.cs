using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;

namespace SondageSite.Models
{
    public class Sondage
    {

        public int IdSondage { get; private set; }
        public string Question { get; private set; }
        public string CodeSuppression { get; private set; }
        public bool Desactiver { get; private set; }
        public DateTime DateCreation { get; private set; }
        public bool ChoixMultiple { get; private set; }

        public Sondage(int id, string question, string codeSuppression, bool desactiver, DateTime dateCreation, bool choixMultiple)
        {
            IdSondage = id;
            Question = question;
            CodeSuppression = codeSuppression;
            Desactiver = desactiver;
            DateCreation = dateCreation;
            ChoixMultiple = choixMultiple;
        }

        public static Sondage ChargerSondageDepuisBDD(int idSondage)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand recupererSondage = connection.CreateCommand();
            recupererSondage.CommandText = "SELECT * FROM Sondage WHERE IdSondage = @IdSondage";
            // Add parameter values
            recupererSondage.Parameters.AddWithValue("@IdSondage", idSondage);
            SqlDataReader reader = recupererSondage.ExecuteReader();

            // On avance sur la première ligne
            reader.Read();

            // IdSondage
            string question = (string)reader["Question"];
            string codeSuppression = (string)reader["CodeSuppression"];
            bool desactiver = (bool)reader["Desactiver"];
            DateTime dateCreation = (DateTime)reader["DateCreation"];
            bool choixMultiple = (bool)reader["ChoixMultiple"];

            if (connection.State == ConnectionState.Open)
                connection.Close();

            Sondage monSondage = new Sondage(idSondage, question, codeSuppression, desactiver, dateCreation, choixMultiple);
            return monSondage;
        }

        public static int InsererSondageEnBdd(string question, string codeSuppression, bool desactiver, bool choixMultiple)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand ajoutQuestion = connection.CreateCommand();
            ajoutQuestion.CommandText = "INSERT INTO Sondage (Question, CodeSuppression, Desactiver, DateCreation, ChoixMultiple) OUTPUT INSERTED.IdSondage VALUES (@Question, @CodeSuppression, @Desactiver, GETDATE(), @ChoixMultiple)";
            // Add parameter values
            ajoutQuestion.Parameters.AddWithValue("@Question", question);
            ajoutQuestion.Parameters.AddWithValue("@CodeSuppression", codeSuppression);
            ajoutQuestion.Parameters.AddWithValue("@Desactiver", desactiver);
            ajoutQuestion.Parameters.AddWithValue("@ChoixMultiple", choixMultiple);
            // Get the inserted id
            int insertedID = (int)ajoutQuestion.ExecuteScalar();
            if (connection.State == ConnectionState.Open)
                connection.Close();
            // Debug.Write("ok");
            return insertedID;

            /*

                        public class StudentDBHandle
                    {
                        private SqlConnection con;
                        private void connection()
                        {
                            string constring = ConfigurationManager.ConnectionStrings["studentconn"].ToString();
                            con = new SqlConnection(constring);
                        }

                        // **************** ADD NEW STUDENT *********************
                        public bool AddStudent(StudentModel smodel)
                        {
                            connection();
                            SqlCommand cmd = new SqlCommand("AddNewStudent", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Name", smodel.Name);
                            cmd.Parameters.AddWithValue("@City", smodel.City);
                            cmd.Parameters.AddWithValue("@Address", smodel.Address);

                            con.Open();
                            int i = cmd.ExecuteNonQuery();
                            con.Close();

                            if (i >= 1)
                                return true;
                            else
                                return false;
                        }

                                   int bookID = 0;
                                   SqlCommand takeBookId = connection.CreateCommand();
                                   takeBookId.CommandText = "SELECT IDBook FROM Book WHERE TitleBook = @TitleBook";
                                   takeBookId.Parameters.AddWithValue("@TitleBook", book.Titre);
                                   bookID = (int)takeBookId.ExecuteScalar();

                                   SqlCommand secondInsert = connection.CreateCommand();
                                   secondInsert.CommandText = "INSERT INTO WriterBook(IDWriter, IDBook) VALUES (@IDWriter,@IDBook)";
                                   secondInsert.Parameters.AddWithValue("@IDWriter", writerID);
                                   secondInsert.Parameters.AddWithValue("@IDBook", bookID);
                                   secondInsert.ExecuteNonQuery();

                                   SqlCommand thirdInsert = connection.CreateCommand();
                                   thirdInsert.CommandText = "INSERT INTO BookSubject(IDBook, IDSubject) VALUES (@IDBook,@IDSubject)";
                                   thirdInsert.Parameters.AddWithValue("@IDBook", bookID);
                                   thirdInsert.Parameters.AddWithValue("@IDSubject", subjectID);
                                   thirdInsert.ExecuteNonQuery();

                                   SqlCommand fourthInsert = connection.CreateCommand();
                                   fourthInsert.CommandText = "INSERT INTO PublisherBook(IDBook, IDPublisher, ISBN, NbEx) VALUES (@IDBook, @IDPublisher, @ISBN, @NbEx)";
                                   fourthInsert.Parameters.AddWithValue("@IDBook", bookID);
                                   fourthInsert.Parameters.AddWithValue("@IDPublisher", PublisherId);
                                   fourthInsert.Parameters.AddWithValue("@ISBN", book.ISBN1);
                                   fourthInsert.Parameters.AddWithValue("@NbEx", book.NbEx);
                                   fourthInsert.ExecuteNonQuery();
                                   connection.Close();*/
        }
    }
}