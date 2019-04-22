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
        {
            Sondage = sondage;
            Options = options;
        }
    }
}