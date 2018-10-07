using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	/// <summary>
	/// Contient des outils relatifs aux traitements des chaînes de caractères
	/// </summary>
	public static partial class Outils
    {
        /// <summary>
        /// Classe de caractères d'une expression régulière correspondant aux lettres non-accentuées
        /// </summary>
        private const string c_RegEx_LettresNonAccentuees = "A-Za-z";

        /// <summary>
        /// Classe de caractères d'une expression régulière correspondant aux lettres accentuées ou non
        /// </summary>
        private const string c_RegEx_ToutesLesLettres = c_RegEx_LettresNonAccentuees + c_LettresAccentuees;

        /// <summary>
        /// Classe de caractères d'une expression régulière correspondant aux chiffres décimaux
        /// </summary>
        private const string c_RegEx_Chiffres = "0-9";

        /// <summary>
        /// Stocke les expressions régulières utilisées par la méthode de test de la validité de nom de "chose"
        /// </summary>
        private static Regex[] s_ValidateursNom = null;

        /// <summary>
        /// Vérifie la validité des caractères d'un nom de "chose"
        /// </summary>
        /// <param name="Nom">Nom à tester</param>
        /// <param name="SuffixeNumeriqueAutorise">Indique si un suffixe numérique (entier positif) est autorisé ou pas</param>
        /// <returns>Vrai si le nom à tester est null, vide ou contient uniquement les caractères admissibles, sinon faux</returns>
        public static bool TesterValiditeNom(string Nom, bool SuffixeNumeriqueAutorise = false)
        {
            if (string.IsNullOrEmpty(Nom)) return true;
            if (s_ValidateursNom == null)
            {
                s_ValidateursNom = new Regex[]
                {
                    new Regex(
                        "^[" + c_RegEx_ToutesLesLettres + "]+"
                        + "([\\s'-][" + c_RegEx_ToutesLesLettres + "]+)*"
                        + "$", RegexOptions.Compiled),
                    new Regex(
                        "^[" + c_RegEx_ToutesLesLettres + "]+"
                        + "([\\s'-][" + c_RegEx_ToutesLesLettres + "]+)*"
                        + "([\\s][" + c_RegEx_Chiffres + "]+){0,1}"
                        + "$", RegexOptions.Compiled)
                };
            }
            return s_ValidateursNom[SuffixeNumeriqueAutorise ? 0 : 1].IsMatch(Nom);
        }

        /// <summary>
        /// Vérifie si les deux noms sont égaux, ne tenant compte que des lettres et chiffres, sans distinction entre minuscule et majuscule
        /// </summary>
        /// <param name="Nom1">Nom à comparer</param>
        /// <param name="Nom2">Autre nom auquel on compare le premier nom spécifié</param>
        /// <returns>Vrai en cas d'égalité, sinon faux</returns>
        public static bool TesterEgaliteNoms(string Nom1, string Nom2)
        {
            return ComparerNoms(Nom1, Nom2) == 0;
        }

        /// <summary>
        /// Compare les deux noms, ne tenant compte que des lettres et chiffres, sans distinction entre minuscule et majuscule
        /// </summary>
        /// <param name="Nom1">Nom à comparer</param>
        /// <param name="Nom2">Autre nom auquel on compare le premier nom spécifié</param>
        /// <returns>-1 si le premier nom est plus "petit" que le second, 1 si le premier est plus "grand" que le second, sinon 0 en cas d'égalité</returns>
        public static int ComparerNoms(string Nom1, string Nom2)
        {
            if (string.IsNullOrEmpty(Nom1)) return string.IsNullOrEmpty(Nom2) ? 0 : -1;
            if (string.IsNullOrEmpty(Nom2)) return 1;
            for (int Indice1 = 0, Indice2 = 0; true;)
            {
                while ((Indice1 < Nom1.Length) && !EstLettre(Nom1[Indice1]) && !EstChiffre(Nom1[Indice1])) Indice1++;
                while ((Indice2 < Nom2.Length) && !EstLettre(Nom2[Indice2]) && !EstChiffre(Nom2[Indice2])) Indice2++;
                if (Indice1 == Nom1.Length) return (Indice2 == Nom2.Length) ? 0 : -1;
                if (Indice2 == Nom2.Length) return 1;
                char Car1 = EnMinuscule(Nom1[Indice1]);
                char Car2 = EnMinuscule(Nom2[Indice2]);
                if (Car1 < Car2) return -1;
                if (Car1 > Car2) return 1;
                Indice1++;
                Indice2++;
            }
        }
    }
}
