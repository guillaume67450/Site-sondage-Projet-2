﻿using System;
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

        public static Sondage ChargerSondageDepuisBDD(int idSondage) // charger le sondage avec idSondage
        {
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand recupererSondage = connection.CreateCommand();
            recupererSondage.CommandText = "SELECT * FROM Sondage WHERE IdSondage = @IdSondage"; // on récupère l'ID
            // Add parameter values
            recupererSondage.Parameters.AddWithValue("@IdSondage", idSondage);
            SqlDataReader reader = recupererSondage.ExecuteReader(); // envoie la commande recuperSondage à la connection grâce au dataReader

            // On avance sur la première ligne du dataReader
            reader.Read();

            // on stocke tous les enregistrements dans des variables
            string question = (string)reader["Question"];
            string codeSuppression = (string)reader["CodeSuppression"];
            bool desactiver = (bool)reader["Desactiver"];
            DateTime dateCreation = (DateTime)reader["DateCreation"];
            bool choixMultiple = (bool)reader["ChoixMultiple"];

            if (connection.State == ConnectionState.Open) 
                connection.Close(); // si la connection est ouverte, on la referme

            Sondage monSondage = new Sondage(idSondage, question, codeSuppression, desactiver, dateCreation, choixMultiple);
            return monSondage; // on renvoie mon sondage comme valeur
        }

        public static int InsererSondageEnBdd(string question, string codeSuppression, bool desactiver, bool choixMultiple)
        {   // déclaration d'InsererSondageEnBdd comme membre statique de la classe Sondage, ainsi les using SondageSite.Models peuvent utiliser Sondage.InsererSondageEnBdd avec ses paramètres
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand ajoutQuestion = connection.CreateCommand(); // création de la commande ajoutQuestion
            ajoutQuestion.CommandText = "INSERT INTO Sondage (Question, CodeSuppression, Desactiver, DateCreation, ChoixMultiple) OUTPUT INSERTED.IdSondage VALUES (@Question, @CodeSuppression, @Desactiver, GETDATE(), @ChoixMultiple)";
            // Add parameter values
            ajoutQuestion.Parameters.AddWithValue("@Question", question);
            ajoutQuestion.Parameters.AddWithValue("@CodeSuppression", codeSuppression);
            ajoutQuestion.Parameters.AddWithValue("@Desactiver", desactiver);
            ajoutQuestion.Parameters.AddWithValue("@ChoixMultiple", choixMultiple);
            // Get the inserted id
            int insertedID = (int)ajoutQuestion.ExecuteScalar(); //ExecuteScalar car récupère une valeur unitaire;
            if (connection.State == ConnectionState.Open)
                connection.Close();
            // Debug.Write("ok");
            return insertedID;




            /* exemple trouvé sur internet : https://www.completecsharptutorial.com/mvc-articles/insert-update-delete-in-asp-net-mvc-5-without-entity-framework.php

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

        public int CountVotants() 
        {  // commande SQL pour compter les votants
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand recupererSondage = connection.CreateCommand();
            recupererSondage.CommandText =
                "SELECT COUNT(distinct v.IdVotant) " +
                "FROM [Option] o, Vote v " +
                "WHERE o.IdSondage = @IdSondage " +
                "AND o.IdOption = v.IdOption";
            // Add parameter values
            recupererSondage.Parameters.AddWithValue("@IdSondage", this.IdSondage);
            int count = (int)recupererSondage.ExecuteScalar(); // récupère une valeur unitaire (à n'utiliser que pour les valeurs uniques)

            return count; // on retourne la valeur du count pour qu'elle soit affichée dans la vue résultat (sous la forme de @Model.Sondage.CountVotants())

        }

        public int CountVotes()
        { // même processus que pour les votants mais avec les votes
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand recupererSondage = connection.CreateCommand();
            recupererSondage.CommandText =
                "SELECT COUNT(*) " +
                "FROM [Option] o, Vote v " +
                "WHERE o.IdSondage = @IdSondage " +
                "AND o.IdOption = v.IdOption";
            // Add parameter values
            recupererSondage.Parameters.AddWithValue("@IdSondage", this.IdSondage);
            int count = (int)recupererSondage.ExecuteScalar();

            return count;

        }

        public void DesactiverSondage()
        { // fonction qui ne retournera rien, publique afin qu'on puisse y accéder depuis le controller
            SqlConnection connection = new SqlConnection(DataAccess.ChaineConnexionBDD);
            connection.Open();

            SqlCommand desactivationSondage = connection.CreateCommand();
            desactivationSondage.CommandText = "UPDATE Sondage SET Desactiver = 'true' WHERE IdSondage = @IdSondage";
            // Add parameter values
            desactivationSondage.Parameters.AddWithValue("@IdSondage", this.IdSondage);
            desactivationSondage.ExecuteNonQuery();
            if (connection.State == ConnectionState.Open)
                connection.Close();
            this.Desactiver = true; // on lui indique que Sondage.Desactiver = true
        }
    }
}