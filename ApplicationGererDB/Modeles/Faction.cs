using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	public class Faction : Entite<Faction, Faction.Champ>
	{
		/// <summary>
		/// Champ décrivant cette faction
		/// </summary>
		public enum Champ
		{
			/// <summary>
			/// Identifiant de cette Faction
			/// </summary>
			Id,
			/// <summary>
			/// name de ce Faction
			/// </summary>
			name
		}

		#region Membres privés
		/// <summary>
		/// Stocke la valeur du champ name
		/// </summary>
		private string m_name;
		
		#endregion

		#region Membres publics
		/// <summary>
		/// name de ce Faction
		/// </summary>
		public string name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					Declencher_SurErreur(this, Champ.name, "name vide ou ne contient que des espaces");
				}
				else
				{
					value = value.Trim();
					if (!string.Equals(value, m_name))
					{
						ModifierChamp(Champ.name, ref m_name, value);
					}
				}
			}
		}

		
		#endregion

		#region Constructeur
		/// <summary>
		/// Constructeur par défaut
		/// </summary>
		public Faction()
            : base()
        {
			m_name = string.Empty;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Id">Identifiant de ce Faction</param>
		/// <param name="name">name de ce Faction</param>
		public Faction(int Id, string name)
            : this()
        {
			DefinirId(Id);
			this.name = name;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
		public Faction(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
			base.Connexion = Connexion;
			if (Enregistrement != null)
			{
				DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "fa_id"));
				this.name = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "fa_name");
			}
		}

		#endregion

		/// <summary>
		/// Permet d'énumérer les entités correspondant aux enregistrements énumérés
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
		/// <returns>Enumération des entités issues des enregistrements énumérés</returns>
		public static IEnumerable<Faction> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
		{
			return Enumerer(Enregistrements, Enregistrement => new Faction(Connexion, Enregistrement));
		}

		
		#region Méthodes relatives au gestionnaire d'entités pour base de données MySQL
		/// <summary>
		/// Méthode retournant le name de la table principale de ce type d'entités
		/// </summary>
		/// <returns>name de la table principale de ce type d'entités</returns>
		public override string NomDeLaTablePrincipale
		{
			get
			{
				return "faction";
			}
		}

		/// <summary>
		/// Clause d'assignation utilisable dans une requête INSERT/UPDATE
		/// </summary>
		public override PDSGBD.MyDB.CodeSql ClauseAssignation
		{
			get
			{
				return new PDSGBD.MyDB.CodeSql("name = {0}", m_name);
			}
		}
		
		#endregion

	}
}
