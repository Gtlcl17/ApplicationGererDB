using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
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
        /// Chaîne contenant toutes les lettres accentuées en majuscule
        /// </summary>
        private const string c_LettresAccentueesEnMajuscule = "ÁÀÂÄÃÅÉÈÊËÍÌÎÏÓÒÔÖÕÚÙÛÜŮÝŶŸÇŃÑĆĈĤĴĹŔŚŜŞŴŹÆŒ";

        /// <summary>
        /// Chaîne contenant toutes les lettres accentuées en minuscule
        /// </summary>
        private const string c_LettresAccentueesEnMinuscule = "áàâäãåéèêëíìîïóòôöõúùûüůýŷÿçńñćĉĥĵĺŕśŝşŵźæœ";

        /// <summary>
        /// Classe de caractères d'une expression régulière correspondant aux lettres accentuées
        /// </summary>
        private const string c_LettresAccentuees = c_LettresAccentueesEnMajuscule + c_LettresAccentueesEnMinuscule;

        /// <summary>
        /// Style des nombres réels
        /// </summary>
        private const NumberStyles c_StyleNumeriqueReel = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;

        /// <summary>
        /// Culture anglaise
        /// <para>Utilisée pour la gestion du séparateur décimal par le caractère .</para>
        /// </summary>
        private static readonly CultureInfo c_CultureAnglaise = CultureInfo.GetCultureInfo("EN-US");

        /// <summary>
        /// Teste si le caractère spécifié est une lettre (en minuscule ou en majuscule, accentuée ou non)
        /// </summary>
        /// <param name="Caractere">Caractère à tester</param>
        /// <returns>Vrai si le caractère est une lettre, sinon faux</returns>
        public static bool EstLettre(char Caractere)
        {
            return ((Caractere >= 'A') && (Caractere <= 'Z'))
                || ((Caractere >= 'a') && (Caractere <= 'z'))
                || c_LettresAccentuees.Contains(Caractere);
        }

        /// <summary>
        /// Teste si le caractère spécifié est une lettre en majuscule (accentuée ou non)
        /// </summary>
        /// <param name="Caractere">Caractère à tester</param>
        /// <returns>Vrai si le caractère est une lettre majuscule, sinon faux</returns>
        public static bool EstLettreMajuscule(char Caractere)
        {
            return ((Caractere >= 'A') && (Caractere <= 'Z'))
                || c_LettresAccentueesEnMajuscule.Contains(Caractere);
        }

        /// <summary>
        /// Teste si le caractère spécifié est une lettre en minuscule (accentuée ou non)
        /// </summary>
        /// <param name="Caractere">Caractère à tester</param>
        /// <returns>Vrai si le caractère est une lettre minuscule, sinon faux</returns>
        public static bool EstLettreMinuscule(char Caractere)
        {
            return ((Caractere >= 'a') && (Caractere <= 'z'))
                || c_LettresAccentueesEnMinuscule.Contains(Caractere);
        }

        /// <summary>
        /// Retourne le caractère spécifié après sa transformation si nécessaire en minuscule (en prenant en compte et conservant les accents)
        /// </summary>
        /// <param name="Caractere">Caractère à transformer</param>
        /// <returns>Caractère correspondant au caractère spécifié, mais écrit en minuscule</returns>
        public static char EnMinuscule(char Caractere)
        {
            if ((Caractere >= 'A') && (Caractere <= 'Z')) return (char)((int)'a' + ((int)Caractere - (int)'A'));
            int Indice = c_LettresAccentueesEnMajuscule.IndexOf(Caractere);
            if (Indice >= 0) return c_LettresAccentueesEnMinuscule[Indice];
            return Caractere;
        }

        /// <summary>
        /// Retourne le caractère spécifié après sa transformation si nécessaire en majuscule (en prenant en compte et conservant les accents)
        /// </summary>
        /// <param name="Caractere">Caractère à transformer</param>
        /// <returns>Caractère correspondant au caractère spécifié, mais écrit en majuscule</returns>
        public static char EnMajuscule(char Caractere)
        {
            if ((Caractere >= 'a') && (Caractere <= 'z')) return (char)((int)'A' + ((int)Caractere - (int)'a'));
            int Indice = c_LettresAccentueesEnMinuscule.IndexOf(Caractere);
            if (Indice >= 0) return c_LettresAccentueesEnMajuscule[Indice];
            return Caractere;
        }

        /// <summary>
        /// Retourne le chaîne spécifiée après sa transformation si nécessaire en minuscule (en prenant en compte et conservant les accents)
        /// </summary>
        /// <param name="Chaine">Chaîne à transformer</param>
        /// <returns>Chaîne correspondant à celle spécifié, mais écrite en minuscule</returns>
        public static string EnMinuscule(string Chaine)
        {
            StringBuilder Resultat = new StringBuilder();
            for (int Indice = 0; Indice < Chaine.Length; Indice++)
            {
                Resultat.Append(EnMinuscule(Chaine[Indice]));
            }
            return Resultat.ToString();
        }

        /// <summary>
        /// Retourne la chaîne spécifiée après sa transformation si nécessaire en majuscule (en prenant en compte et conservant les accents)
        /// </summary>
        /// <param name="Chaine">Chaîne à transformer</param>
        /// <returns>Chaîne correspondant à celle spécifié, mais écrite en majuscule</returns>
        public static string EnMajuscule(string Chaine)
        {
            StringBuilder Resultat = new StringBuilder();
            for (int Indice = 0; Indice < Chaine.Length; Indice++)
            {
                Resultat.Append(EnMajuscule(Chaine[Indice]));
            }
            return Resultat.ToString();
        }

        /// <summary>
        /// Teste si le caractère spécifié est un chiffre
        /// </summary>
        /// <param name="Caractere">Caractère à tester</param>
        /// <returns>Vrai si le caractère est un chiffre, sinon faux</returns>
        public static bool EstChiffre(char Caractere)
        {
            return (Caractere >= '0') && (Caractere <= '9');
        }

        /// <summary>
        /// Tente de convertir le texte en valeur numérique réelle
        /// <para>Le séparateur décimal pouvant être aussi bien le . que la ,</para>
        /// <para>Ignore les espaces de début et de fin de chaîne</para>
        /// </summary>
        /// <param name="Texte">Texte à convertir</param>
        /// <param name="Valeur">Valeurt convertie</param>
        /// <returns>Vrai si la conversion est réussie, sinon faux</returns>
        public static bool Convertir(string Texte, out float Valeur)
        {
            return float.TryParse(Texte.Trim().Replace(',', '.'), c_StyleNumeriqueReel, c_CultureAnglaise, out Valeur);
        }

        /// <summary>
        /// Tente de convertir le texte en valeur numérique réelle
        /// <para>Le séparateur décimal pouvant être aussi bien le . que la ,</para>
        /// <para>Ignore les espaces de début et de fin de chaîne</para>
        /// </summary>
        /// <param name="Texte">Texte à convertir</param>
        /// <param name="Valeur">Valeurt convertie</param>
        /// <returns>Vrai si la conversion est réussie, sinon faux</returns>
        public static bool Convertir(string Texte, out double Valeur)
        {
            return double.TryParse(Texte.Trim().Replace(',', '.'), c_StyleNumeriqueReel, c_CultureAnglaise, out Valeur);
        }

        /// <summary>
        /// Tente de convertir le texte en valeur numérique réelle
        /// <para>Le séparateur décimal pouvant être aussi bien le . que la ,</para>
        /// <para>Ignore les espaces de début et de fin de chaîne</para>
        /// </summary>
        /// <param name="Texte">Texte à convertir</param>
        /// <param name="Valeur">Valeurt convertie</param>
        /// <returns>Vrai si la conversion est réussie, sinon faux</returns>
        public static bool Convertir(string Texte, out decimal Valeur)
        {
            return decimal.TryParse(Texte.Trim().Replace(',', '.'), c_StyleNumeriqueReel, c_CultureAnglaise, out Valeur);
        }

        /// <summary>
        /// Tente de convertir le texte en valeur date/heure
        /// <para>Se base sur le format DD/MM/YYYY HH:mm[:ss]</para>
        /// <para>Ignore les espaces de début et de fin de chaîne</para>
        /// </summary>
        /// <param name="Texte">Texte à convertir</param>
        /// <param name="Valeur">Valeurt convertie</param>
        /// <returns>Vrai si la conversion est réussie, sinon faux</returns>
        public static bool Convertir(string Texte, out DateTime Valeur)
        {
            Valeur = default(DateTime);
            if (string.IsNullOrWhiteSpace(Texte)) return false;
            Texte = Texte.Trim();
            if ((Texte.Length < 12) || (Texte.Length > 19)) return false;
            int Entier;
            StringBuilder Contenu = new StringBuilder(Texte);
            List<int> Valeurs = Contenu.Replace('/', ' ').Replace(':', ' ').Replace('-', ' ').ToString().Split(' ').Select(Chaine => int.TryParse(Chaine, out Entier) ? Entier : -1).TakeWhile(v => v >= 0).ToList();
            if ((Valeurs.Count < 5) || (Valeurs.Count > 6)) return false;
            if (Valeurs.Count == 5) Valeurs.Add(0);
            if ((Valeurs[1] < 1) || (Valeurs[1] > 12) || (Valeurs[0] < 1) || (Valeurs[0] > DateTime.DaysInMonth(Valeurs[2], Valeurs[1])) || (Valeurs[3] > 23) || (Valeurs[4] > 59) || (Valeurs[5] > 59)) return false;
            Valeur = new DateTime(Valeurs[2], Valeurs[1], Valeurs[0], Valeurs[3], Valeurs[4], Valeurs[5]);
            return true;
        }
    }
}
