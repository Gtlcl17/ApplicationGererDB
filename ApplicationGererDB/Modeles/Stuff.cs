using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
    public class Stuff : Entite<Stuff, Stuff.Champ>
    {
       
        /// <summary>
        /// Champ décrivant cette Stuff
        /// </summary>
        public enum Champ
        {
            /// <summary>
            /// Identifiant de cette Stuff
            /// </summary>
            Id,
            /// <summary>
            /// Nom de cette Stuff
            /// </summary>
            Name,
            /// <summary>
            /// Mot de passe de cette Stuff
            /// </summary>
            Cost,
        }

        #region Membres privés
        /// <summary>
        /// Stocke la valeur du champ Name
        /// </summary>
        private string m_Name;
        /// <summary>
        /// Stocke la valeur du champ Cost
        /// </summary>
        private int m_Cost;
		/// <summary>
		/// Code SQL correspondant à d'éventuelles conditions à utiliser lors de l'énumération d'appareils présents dans cette zône
		/// </summary>
		private PDSGBD.MyDB.CodeSql m_ConditionsPourStats;
		/// <summary>
		/// Stocke les utilisations concernant ce type d'appareils présents dans cette zône
		/// </summary>
		private List<Stat> m_Stats;
		#endregion

		#region Membres publics
		/// <summary>
		/// Nom de cette Stuff
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
        /// Valeur de cette Stuff
        /// </summary>
        public int Cost
        {
            get
            {
                return m_Cost;
            }

            set
            {
                if(value <=0)
                {
                    Declencher_SurErreur(this, Champ.Cost, "Le coût doit être supérieure à 0!");
                }
                else
                {
					if(!int.Equals(value, m_Cost))
                    ModifierChamp(Champ.Cost, ref m_Cost, value);
                }
            }
        }

		/// <summary>
		/// Code SQL correspondant à d'éventuelles conditions à utiliser lors de l'énumération de stats pour ce stuff
		/// </summary>
		public PDSGBD.MyDB.CodeSql ConditionsPourAppareilsPresents
		{
			get
			{
				return m_ConditionsPourStats;
			}
			set
			{
				if (value == null)
				{
					if (m_ConditionsPourStats == null) return;
					m_ConditionsPourStats = null;
				}
				else
				{
					if ((m_ConditionsPourStats != null)
						&& value.Code.Equals(m_ConditionsPourStats.Code))
						return;
					m_ConditionsPourStats = value;
				}
				m_Stats.Clear();
			}
		}

		/// <summary>
		/// Appareils présents dans cette zône
		/// </summary>
		public IEnumerable<Stat> Stats
		{
			get
            {
                {
                    return EnumererStats();
                }
            }
		}


		/// <summary>
		/// Permet d'énumérer les entités de type AppareilPresent relatives à cette zône
		/// </summary>
		/// <returns>Enumération des appareils présents de cette zône</returns>
		private IEnumerable<Stat> EnumererStats()
		{
			if (base.Connexion == null) return new Stat[0];
			return Stat.Enumerer(Connexion, Connexion.Enumerer(
				@"SELECT stuff.id AS `stuff.id`,
                         stuff.name AS `stuff.name`,
                         statstuff.id AS `statstuff.id`,
                         
                         stat.id AS `stat.id`,
                         stat.name AS `stat.name`,
                         stat.value AS `stat.value`,
                         
                  FROM stuff
                  INNER JOIN statstuff ON stuff.id = stastuff.id_stuff
                  INNER JOIN stat ON statstuff.id_stat = stat.id
                  WHERE (stuff.id = {0}) AND ({1})",
				Id,
				(m_ConditionsPourStats != null) ? m_ConditionsPourStats : new PDSGBD.MyDB.CodeSql("TRUE")));
		}
		#endregion

		#region Constructeurs
		/// <summary>
		/// Constructeur par défaut
		/// </summary>
		public Stuff()
            : base()
        {
            m_Name = string.Empty;
            m_Cost = 0;
			m_Stats = new List<Stat>();
		}

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Id">Identifiant de cette Stuff</param>
        /// <param name="Name">Nom de cette Stuff</param>
        /// <param name="Cost">Valeur de cette Stuff</param>
        public Stuff(int Id, string Name, int Cost)
            : this()
        {
            DefinirId(Id);
            this.Name = Name;
            this.Cost = Cost;
        }

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        /// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
        public Stuff(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
            base.Connexion = Connexion;
            if (Enregistrement != null)
            {
                DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "id"));
                this.Name = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "name");
                this.Cost = Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "cost");
			}
        }

        /// <summary>
        /// Permet d'énumérer les entités correspondant aux enregistrements énumérés
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        /// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
        /// <returns>Enumération des entités issues des enregistrements énumérés</returns>
        public IEnumerable<Stuff> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
        {
            return Enumerer(Enregistrements, Enregistrement => new Stuff(Connexion, Enregistrement));
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
                return "Stuffs";
            }
        }

        /// <summary>
        /// Clause d'assignation utilisable dans une requête INSERT/UPDATE
        /// </summary>
        public override PDSGBD.MyDB.CodeSql ClauseAssignation
        {
            get
            {
				return new PDSGBD.MyDB.CodeSql("name = {0}, cost = {1}", m_Name, m_Cost);
            }
        }
        
		/// <summary>
        /// Permet de supprimer tous les changements de Stuffus liés à ce projet
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        public override void SupprimerEnCascade(PDSGBD.MyDB Connexion)
        {
            Connexion.Executer("DELETE FROM Stuffs WHERE id = {0}", Id);
        }
        #endregion

    }
}
