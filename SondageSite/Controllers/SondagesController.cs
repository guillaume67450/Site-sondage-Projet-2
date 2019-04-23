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

        public ActionResult Accueil() // action qui retourne la vue de la page d'accueil
        {
            return View();
        }

        public ActionResult ReussiteSuppression() // action qui retourne la vue de la page de confirmation de désactivation du sondage
        {
            return View();
        }

        public ActionResult ErreurCodeSuppression() // action qui retourne la vue de la page d'erreur dans le code de désactivation du sondage
        {
            return View();
        }

        public ActionResult ErreurSondageDesactive() // action qui retourne la vue de la page d'erreur lorsqu'on tente d'aller sur la page de vote d'un sondage désactivé
        {
            return View();
        }

        public ActionResult ErreurSondageMoinsDe10Caracs() // action qui retourne la vue de la page d'erreur si on pose une question de moins de 10 caractères ou des réponses de moins de 3 caractères
        {
            return View();
        }
        

        public ActionResult SondageCreation() // action qui retourne la vue de la page de création du sondage
        {
            return View();
        }

        //public RedirectToRouteResult AjoutSondage(string question, string reponse1, string reponse2, string reponse3, string reponse4) --> méthode intiale : pas conservée finalement
        public ActionResult AjoutSondage(string question, string reponse1, string reponse2, string reponse3, string reponse4, string codeSecret, bool reponseMultiple = false)
        { // action d'ajouter un sondage avec les paramètres de la question et des réponses, et initialise la réponse multiple à non activé
            //on autorise la création du sondage seulement si la question contient au moins 10 caractères et les réponses au moins 3 caractères
            if (question.Length > 9 && reponse1.Length > 2 && reponse2.Length > 2 && reponse3.Length > 2 && reponse4.Length > 2)
            {
                // on appelle ici la fonction qui permet de créer un code aléatoire avec xx (ici 20) caractères (voir plus bas)
                string randomCode = GenerateRandomCode(20);

                // insérer instance de sondage avec ses paramètres
                int idSondage = Sondage.InsererSondageEnBdd(question, randomCode, false, reponseMultiple);

                // insérer les différentes réponses en BDD
                Option.InsererOptionEnBdd(idSondage, reponse1);
                Option.InsererOptionEnBdd(idSondage, reponse2);
                Option.InsererOptionEnBdd(idSondage, reponse3);
                Option.InsererOptionEnBdd(idSondage, reponse4);

                // Redirection vers l'action de création du nouveau sondage, spéciale créateur (pour les autres c'est SondageVote)
                return RedirectToAction("SondageCree", new { idSondage = idSondage, codeSecret = codeSecret });
                }
            else
            {   // Redirection vers la page d'erreur pour les sondages ou questions trop courtes
                return RedirectToAction("ErreurSondageMoinsDe10Caracs");
            }
        }

        public ActionResult SondageCree(int idSondage)
        {  // action de création du sondage qui retourne la vue spéciale créateur (SondageCree) avec le modèle sondage et options pour définir l'instance de sondage et de la liste d'options 
            // Get sondage by idSondage
            Sondage monSondage = Sondage.ChargerSondageDepuisBDD(idSondage);
            // Get options by idSondage
            List<Option> optionList = Option.ChargerOptionsDepuisBDD(idSondage);

            // Pour grouper les objets
            SondageEtOptions sondageEtOptions = new SondageEtOptions(monSondage, optionList);

            // envoyer les données à la vue
            return View(sondageEtOptions);
        }

        public ActionResult SondageVote(int idSondage)
        { // on appelle l'action avec comme paramètre l'id de l'instance du sondage
            // Get sondage by id
            Sondage monSondage = Sondage.ChargerSondageDepuisBDD(idSondage);

            // si le sondage est désactivé, on ne va pas plus loin et on renvoie à l'action ci-dessous
            if (monSondage.Desactiver)
            {   
                return RedirectToAction("ErreurSondageDesactive"); 
            }

            // Get options by idSondage
            List<Option> optionList = Option.ChargerOptionsDepuisBDD(idSondage);

            // Grouper les objets
            SondageEtOptions sondageEtOptions = new SondageEtOptions(monSondage, optionList);

            // envoyer les données à la vue
            return View(sondageEtOptions);
        }

        public ActionResult AjoutVote(int idSondage, int? reponse1, int? reponse2, int? reponse3, int? reponse4, int? radioReponse)
        {  // action pour ajouter le vote, appelée avec les paramètres de l'id sondage et des réponses, et le bouton radio
           // insérer la nouvelle instance de votant
            int idVotant = Votant.InsererVotantEnBdd();

            // Ajouter réponses

            if (radioReponse is int idOption)
            {  // on teste si la valeur associé au radio bouton (celle qu'on envoie du formulaire) est un entier, car si elle n'est pas définie alors c'est qu'on a choisi les choix multiples 
                Vote vote = new Vote(idVotant, idOption);
                Vote.InsererVoteEnBdd(vote); // on crée une nouvelle instance de vote avec les valeurs d'id votant et de la réponse choisie
                Option.MettreAJourNbVotes(idOption); // Maj du nombre de vote depuis le modèle option
            }
            else
            { // si on a choisi les choix multiples

                    if (reponse1 is int idOption1)
                { // pareil qu'avant mais avec les choix multiples
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
            }

            

            // Redirection vers l'action sondage résultat qui donne la vue
            return RedirectToAction("SondageResultat", new { idSondage = idSondage });
        }

        public ActionResult SondageResultat(int idSondage)
        {
            // Get sondage by id
            Sondage monSondage = Sondage.ChargerSondageDepuisBDD(idSondage);
            // Get options by idSondage
            List<Option> optionList = Option.ChargerOptionsDepuisBDD(idSondage);

            // Grouper les objets
            SondageEtOptions sondageEtOptions = new SondageEtOptions(monSondage, optionList);

            // envoyer les données à la vue
            return View(sondageEtOptions);
        }

        // Helpers
        private string GenerateRandomCode(int length)
        { // fonction permettant de générer un code aléatoire de xx caractères
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            { // on boucle jusqu'à ce qu'on arrive à la longueur demandée en ajoutant à chaque fois un caractère aléatoire
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }

        public ActionResult SondageSuppression(int idSondage, string CodeSuppression)
        {  // action sondage suppression permettant de désactiver le sondage à l'aide du code suppression à rajouter dans l'url
            // Get sondage by id
            Sondage monSondage = Sondage.ChargerSondageDepuisBDD(idSondage);
            
            if (monSondage.CodeSuppression == CodeSuppression)
            {
                monSondage.DesactiverSondage();

                // Retour à l'accueil après avoir été sur la page de confirmation du bon fonctionnement de la suppression

            return RedirectToAction("ReussiteSuppression");
            }

            else
            {   // si on n'a pas saisi la bonne url pour la suppression
                return RedirectToAction("ErreurCodeSuppression");
            }
        }

    }
}