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
        #region Sous-type publique
        /// <summary>
        /// Objet encapsulant un code SQL pour qu'il puisse être injecter sans aucune transformation dans une requête SQL ou dans un code SQL
        /// </summary>
        public class CodeSql
        {
            /// <summary>
            /// Référence globale sur un objet de type CodeSql ayant pour but d'exposer un code vide
            /// </summary>
            private static readonly CodeSql s_Vide = new CodeSql();

            /// <summary>
            /// Code SQL vide
            /// </summary>
            public static CodeSql Vide
            {
                get
                {
                    s_Vide.m_Code = string.Empty;
                    return s_Vide;
                }
            }

            /// <summary>
            /// Membre privé permettant de stocker le texte de ce code SQL
            /// </summary>
            private string m_Code;

            /// <summary>
            /// Texte du code SQL
            /// </summary>
            public string Code
            {
                get
                {
                    return m_Code;
                }
                set
                {
                    if (value != null)
                    {
                        m_Code = value;
                    }
                }
            }

            /// <summary>
            /// Constructeur par défaut
            /// </summary>
            public CodeSql()
            {
                m_Code = string.Empty;
            }

            /// <summary>
            /// Constructeur spécifique
            /// </summary>
            /// <param name="Code">Code SQL pouvant contenir des parties variables</param>
            /// <param name="Valeurs">Valeurs des parties variables</param>
            public CodeSql(string Code, params object[] Valeurs)
                : this()
            {
                this.Code = FormaterEnSql(Code, Valeurs);
            }

            /// <summary>
            /// Réécriture de la méthode permettant de retourner le code SQL tel quel
            /// </summary>
            /// <returns>Chaîne correspondant exactement au code SQL</returns>
            public override string ToString()
            {
                return m_Code;
            }
        }
        #endregion

        #region Méthode statique publique
        /// <summary>
        /// Permet de créer un code SQL qui pourra être utiliser comme partie variable dans une requête SQL ou dans un code SQL
        /// </summary>
        /// <param name="Code">Code SQL pouvant contenir des parties variables</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Nouvel objet de type CodeSql prêt à être injecter sans aucune transformation dans une requête SQL ou dans un code SQL</returns>
        public static CodeSql CreerCodeSql(string Code, params object[] Valeurs)
        {
            return new CodeSql(Code, Valeurs);
        }
        #endregion
    }
}
