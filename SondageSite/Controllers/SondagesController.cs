using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SondageSite.Models;

namespace SondageSite.Controllers
{
    public class SondagesController : Controller
    {
        // GET: Sondages

        public ActionResult Accueil()
        {
            return View();
        }

        public ActionResult SondageCreation()
        {
            return View();
        }

        //public RedirectToRouteResult AjoutSondage(string question, string reponse1, string reponse2, string reponse3, string reponse4)
        public ActionResult AjoutSondage(string question, string reponse1, string reponse2, string reponse3, string reponse4, bool reponseMultiple = false)
        {
            string randomCode = GenerateRandomCode(20);

            // Insert sondage
            int idSondage = Sondage.InsererSondageEnBdd(question, randomCode, false, reponseMultiple);

            // Insert options
            Option.InsererOptionEnBdd(idSondage, reponse1);
            Option.InsererOptionEnBdd(idSondage, reponse2);
            Option.InsererOptionEnBdd(idSondage, reponse3);
            Option.InsererOptionEnBdd(idSondage, reponse4);

            // Redirect to vote page
            return RedirectToAction("SondageVote", new { idSondage = idSondage });
        }

        public ActionResult SondageVote(int idSondage)
        {
            // Get sondage by id
            Sondage monSondage = Sondage.ChargerSondageDepuisBDD(idSondage);
            // Get options by idSondage
            List<Option> optionList = Option.ChargerOptionsDepuisBDD(idSondage);

            // Group objects
            SondageEtOptions sondageEtOptions = new SondageEtOptions(monSondage, optionList);

            // Send data to view
            return View(sondageEtOptions);
        }

        public ActionResult AjoutVote(int idSondage, int? reponse1, int? reponse2, int? reponse3, int? reponse4, int? radioReponse)
        {
            // TODO Check desactiver before vote
            int idVotant = Votant.InsererVotantEnBdd();

            // Ajouter réponses

            if (radioReponse is int idOption)
            {
                Vote vote = new Vote(idVotant, idOption);
                Vote.InsererVoteEnBdd(vote);
                Option.MettreAJourNbVotes(idOption);
                return View();
            }

            if (reponse1 is int idOption1)
            {
                Vote vote = new Vote(idVotant, idOption1);
                Vote.InsererVoteEnBdd(vote);
                Option.MettreAJourNbVotes(idOption1);
            }

            if (reponse2 is int idOption2)
            {
                Vote vote = new Vote(idVotant, idOption2);
                Vote.InsererVoteEnBdd(vote);
                Option.MettreAJourNbVotes(idOption2);
            }

            if (reponse3 is int idOption3)
            {
                Vote vote = new Vote(idVotant, idOption3);
                Vote.InsererVoteEnBdd(vote);
                Option.MettreAJourNbVotes(idOption3);
            }

            if (reponse4 is int idOption4)
            {
                Vote vote = new Vote(idVotant, idOption4);
                Vote.InsererVoteEnBdd(vote);
                Option.MettreAJourNbVotes(idOption4);
            }

            // Redirect to result page
            return RedirectToAction("SondageResultat", new { idSondage = idSondage });
        }

        public ActionResult SondageResultat(int idSondage)
        {
            // Get sondage by id
            Sondage monSondage = Sondage.ChargerSondageDepuisBDD(idSondage);
            // Get options by idSondage
            List<Option> optionList = Option.ChargerOptionsDepuisBDD(idSondage);

            // Group objects
            SondageEtOptions sondageEtOptions = new SondageEtOptions(monSondage, optionList);

            // Send data to view
            return View(sondageEtOptions);
        }

        // Helpers
        private string GenerateRandomCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
        protected void SetTempDataValue<T>(string key, T value)
        {
            TempData[key] = value;
        }


        protected T GetTempDataValue<T>(string key)
        {
            return (T)TempData[key];
        }


        protected T GetTempDataValue<T>(string key, bool persist)
        {
            T result = GetTempDataValue<T>(key);
            if (persist)
                SetTempDataValue<T>(key, result);
            return result;
        }

    }
}