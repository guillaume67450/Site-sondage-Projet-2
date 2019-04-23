using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SondageSite.Models
{
    public class DataAccess
    { // chaine utilisée pour accéder à la CDD, peut être modifiée si on utlise autre chose que SQL Server
        public const string ChaineConnexionBDD = @"Server=.\SQLEXPRESS;Database=SONDAGE;Integrated Security = true";
        //public const string ChaineConnexionBDD = @"Server=localhost;Database=SONDAGE;Trusted_Connection=True";
    }
}