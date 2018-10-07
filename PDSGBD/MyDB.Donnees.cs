using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PDSGBD
{
    /// <summary>
    /// Permet de gérer une connexion à un serveur MySql afin d'y travailler sur une base de données
    /// </summary>
    public partial class MyDB
    {
        #region Membres, constantes et méthodes privées statiques
        /// <summary>
        /// Représente les informations culturelles anglo-américaines
        /// </summary>
        private static readonly System.Globalization.CultureInfo s_CultureAnglaise = System.Globalization.CultureInfo.GetCultureInfo("EN-US");

        /// <summary>
        /// Détermine si la requête spécifiée est de type consultation (SELECT ou SHOW) ou non
        /// </summary>
        /// <param name="Requete">Requête à tester</param>
        /// <returns>Vrai si la requête spécifiée est de type consultation, sinon faux</returns>
        private static bool EstRequeteConsultation(string Requete)
        {
            Requete = Requete.TrimStart();
            return Requete.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase)
                || Requete.StartsWith("SHOW", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Détermine si la requête spécifiée est de type INSERT INTO ou non
        /// </summary>
        /// <param name="Requete">Requête à tester</param>
        /// <returns>Vrai si la requête spécifiée est de type INSERT INTO, sinon faux</returns>
        private static bool EstRequeteInsertion(string Requete)
        {
            return Requete.TrimStart().StartsWith("INSERT", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Permet de créeer l'objet servant à l'exécution de requête SQL
        /// </summary>
        /// <param name="Requete"></param>
        /// <param name="Valeurs"></param>
        /// <returns></returns>
        private MySqlCommand CreerCommande(string Requete, params object[] Valeurs)
        {
            MySqlCommand Commande = new MySqlCommand();
            Commande.Connection = m_Connexion;
            Commande.CommandText = FormaterEnSql(Requete, Valeurs);
            return Commande;
        }

        /// <summary>
        /// Remplace les caractères dangereux au sein d'une chaîne qui devra être injectée en SQL entre guillemets
        /// </summary>
        /// <param name="Chaine">Chaîne</param>
        /// <returns>Chaîne transformée pour son injection en SQL</returns>
        private static string ChaineEnSql(string Chaine)
        {
            return Chaine.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
        #endregion

        #region Méthodes statiques publiques
        /// <summary>
        /// Permet de formater une requête SQL en injectant de manière sécurisée les valeurs de ses parties variables
        /// </summary>
        /// <param name="Requete">Requête SQL</param>
        /// <param name="Valeurs">Valeurs des parties variables de la requête</param>
        /// <returns>Requête SQL après injection</returns>
        public static string FormaterEnSql(string Requete, params object[] Valeurs)
        {
            try
            {
                return string.Format(Requete, Valeurs.Select(Valeur => ValeurEnSql(Valeur)).ToArray());
            }
            catch (Exception Erreur)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(
                    "\nMyDB.FormaterEnSql({0}{1}) a échoué :\n{2}\n",
                    Requete,
                    string.Join("", Valeurs.Select(Valeur => string.Format(", {0}", Valeur))),
                    Erreur.Message));
                return "";
            }
        }

        /// <summary>
        /// Retourne la valeur spécifiée sous forme de chaîne, en garantissant une injection SQL sans erreur
        /// </summary>
        /// <param name="Valeur">Valeur à transformer en chaîne SQL</param>
        /// <returns>Chaîne SQL correspondant à la valeur spécifiée</returns>
        /// <exception cref="ArgumentException">Exception produite quand le type de la valeur n'est pas pris en charge</exception>
        public static string ValeurEnSql(object Valeur)
        {
            if (Valeur == null) return "NULL";
            if (Valeur is sbyte) return Valeur.ToString();
            if (Valeur is byte) return Valeur.ToString();
            if (Valeur is short) return Valeur.ToString();
            if (Valeur is ushort) return Valeur.ToString();
            if (Valeur is int) return Valeur.ToString();
            if (Valeur is uint) return Valeur.ToString();
            if (Valeur is long) return Valeur.ToString();
            if (Valeur is ulong) return Valeur.ToString();
            if (Valeur is float) return ((float)Valeur).ToString(s_CultureAnglaise);
            if (Valeur is double) return ((double)Valeur).ToString(s_CultureAnglaise);
            if (Valeur is decimal) return ((decimal)Valeur).ToString(s_CultureAnglaise);
            if (Valeur is bool) return ((bool)Valeur) ? "TRUE" : "FALSE";
            if (Valeur is DateTime) return string.Format("\"{0}\"", ((DateTime)Valeur).ToString("yyyy-MM-dd HH:mm:ss"));
            if ((Valeur is string) || (Valeur is StringBuilder)) return string.Format("\"{0}\"", ChaineEnSql(Valeur.ToString()));
            if (Valeur is CodeSql) return Valeur.ToString();
            throw new ArgumentException(string.Format(
                "Le type {0} n'est pas acceptable comme type de valeur de champ MySql !",
                Valeur.GetType().FullName));
        }
        #endregion
    }
}
