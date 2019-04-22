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
        {;
            Sondage = sondage;
            Options = options;
        }

        public string Pourcentage (int numOption)
        {
            double nbvotes = Options[numOption].NbVotes;
            double nbTotalVotes = Sondage.CountVotes();
            const double cent = 100;
            double pourcent = nbvotes / nbTotalVotes * cent;
            return  string.Format("{0:0}",pourcent);
        }
    }
}