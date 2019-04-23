using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SondageSite.Models
{
    public class SondageEtOptions
    {
        public Sondage Sondage { get; set; }
        public List<Option> Options { get; set; }

        public SondageEtOptions(Sondage sondage, List<Option> options)
        {; // dans ce modèle on va chercher les instances de la question du sondage et de la liste des réponses
            Sondage = sondage;
            Options = options;
        }

        public string Pourcentage (int numOption)
        {  // objet pourcentage qui va permettre d'afficher les pourcentages dans la page de résultat des sondages
            double nbvotes = Options[numOption].NbVotes;
            double nbTotalVotes = Sondage.CountVotes();
            const double cent = 100;
            double pourcent = nbvotes / nbTotalVotes * cent;
            return  string.Format("{0:0}",pourcent); // on formate le pourcentage pour qu'il puisse renvoyer une valeur correcte
        }
    }
}