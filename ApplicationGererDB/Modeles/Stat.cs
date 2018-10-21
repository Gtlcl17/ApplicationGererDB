using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
    public class Stat : Entite<Stat, Stat.Champ>
    {
       
        /// <summary>
        /// Champ décrivant cette Stat
        /// </summary>
        public enum Champ
        {
            /// <summary>
            /// Identifiant de cette Stat
            /// </summary>
            Id,
            /// <summary>
            /// Nom de cette Stat
            /// </summary>
            Name,
            /// <summary>
            /// Valeur de cette Stat
            /// </summary>
            Value,
        }

        #region Membres privés
        /// <summary>
        /// Stocke la valeur du champ Name
        /// </summary>
        private string m_Name;
        /// <summary>
        /// Stocke la valeur du champ Value
        /// </summary>
        private int m_Value;   
        #endregion

        #region Membres publics
        /// <summary>
        /// Nom de cette Stat
        /// </summary>
        public string Name
        {
            get
            {
                 return m_Name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Declencher_SurErreur(this, Champ.Name, "Nom vide ou ne contient que des espaces");
                }
                else
                {
                    value = value.Trim();
                    if(!string.Equals(value, m_Name))
                    {
                        ModifierChamp(Champ.Name, ref m_Name, value);
                    }
                }
            }
        }

        /// <summary>
        /// Valeur de cette Stat
        /// </summary>
        public int Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                if(value <=0)
                {
                    Declencher_SurErreur(this, Champ.Value, "La valeur doit être supérieure à 0!");
                }
                else
                {
					if(!int.Equals(value, m_Value))
                    ModifierChamp(Champ.Value, ref m_Value, value);
                }
            }
        }
        #endregion
		        
        #region Constructeurs
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Stat()
            : base()
        {
            m_Name = string.Empty;
            m_Value = 0;
        }

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Id">Identifiant de cette Stat</param>
        /// <param name="Name">Nom de cette Stat</param>
        /// <param name="Value">Valeur de cette Stat</param>
        public Stat(int Id, string Name, int Value)
            : this()
        {
            DefinirId(Id);
            this.Name = Name;
            this.Value = Value;
        }

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        /// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
        public Stat(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
            base.Connexion = Connexion;
            if (Enregistrement != null)
            {
                DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "id"));
                this.Name = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "name");
                this.Value = Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "value");
			}
        }

        /// <summary>
        /// Permet d'énumérer les entités correspondant aux enregistrements énumérés
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        /// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
        /// <returns>Enumération des entités issues des enregistrements énumérés</returns>
        public static IEnumerable<Stat> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
        {
            return Enumerer(Enregistrements, Enregistrement => new Stat(Connexion, Enregistrement));
         }
        
        #endregion

        #region Méthodes relatives au gestionnaire d'entités pour base de données MySQL
        
        /// <summary>
        /// Méthode retournant le nom de la table principale de ce type d'entités
        /// </summary>
        /// <returns>Nom de la table principale de ce type d'entités</returns>
        public override string NomDeLaTablePrincipale
        {
            get
            {
                return "stats";
            }
        }

        /// <summary>
        /// Clause d'assignation utilisable dans une requête INSERT/UPDATE
        /// </summary>
        public override PDSGBD.MyDB.CodeSql ClauseAssignation
        {
            get
            {
				return new PDSGBD.MyDB.CodeSql("name = {0}, value = {1}", m_Name, m_Value);
            }
        }
        
		/// <summary>
        /// Permet de supprimer tous les changements de status liés à ce projet
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        public override void SupprimerEnCascade(PDSGBD.MyDB Connexion)
        {
            Connexion.Executer("DELETE FROM stats WHERE id = {0}", Id);
        }
        #endregion

    }
}
