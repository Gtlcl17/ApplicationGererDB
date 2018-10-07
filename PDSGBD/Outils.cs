using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDSGBD_v0
{
    public static class Outils
    {
        private const NumberStyles c_StyleNumerique = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;

        private static readonly CultureInfo c_CultureAnglaise = CultureInfo.GetCultureInfo("EN-US");

        public static bool Convertir(string Texte, out float Valeur)
        {
            return float.TryParse(Texte.Replace(',', '.'), c_StyleNumerique, c_CultureAnglaise, out Valeur);
        }

        public static bool Convertir(string Texte, out double Valeur)
        {
            return double.TryParse(Texte.Replace(',', '.'), c_StyleNumerique, c_CultureAnglaise, out Valeur);
        }

        public static bool Convertir(string Texte, out decimal Valeur)
        {
            return decimal.TryParse(Texte.Replace(',', '.'), c_StyleNumerique, c_CultureAnglaise, out Valeur);
        }

        public static bool Convertir(string Texte, out sbyte Valeur)
        {
            return sbyte.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out byte Valeur)
        {
            return byte.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out short Valeur)
        {
            return short.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out ushort Valeur)
        {
            return ushort.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out int Valeur)
        {
            return int.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out uint Valeur)
        {
            return uint.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out long Valeur)
        {
            return long.TryParse(Texte, out Valeur);
        }

        public static bool Convertir(string Texte, out ulong Valeur)
        {
            return ulong.TryParse(Texte, out Valeur);
        }

        public static bool EstNumerique_float(object Valeur)
        {
            if (Valeur is float)
            {
                float ValeurNumerique = (float)Valeur;
                return !float.IsNaN(ValeurNumerique) && !float.IsInfinity(ValeurNumerique);
            }
            else
            {
                return false;
            }
        }

        public static bool EstNumerique_double(object Valeur)
        {
            if (Valeur is double)
            {
                double ValeurNumerique = (double)Valeur;
                return !double.IsNaN(ValeurNumerique) && !double.IsInfinity(ValeurNumerique);
            }
            else
            {
                return false;
            }
        }

        public static bool EstNumerique_decimal(object Valeur)
        {
            return (Valeur is decimal);
        }
    }
}
