using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	public class Role : Entite<Role, Role.Champ>
	{
		/// <summary>
		/// Champ décrivant ce rôle
		/// </summary>
		public enum Champ
		{
			/// <summary>
			/// Identifiant de ce role
			/// </summary>
			Id,
			/// <summary>
			/// Nom de ce role
			/// </summary>
			Nom
		}

		#region Membres privés
		/// <summary>
		/// Stocke la valeur du champ Nom
		/// </summary>
		private string m_Nom;
		
		#endregion

		#region Membres publics
		/// <summary>
		/// Nom de ce role
		/// </summary>
		public string Nom
		{
			get
			{
				return m_Nom;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					Declencher_SurErreur(this, Champ.Nom, "Titre vide ou ne contient que des espaces");
				}
				else
				{
					value = value.Trim();
					if (!string.Equals(value, m_Nom))
					{
						ModifierChamp(Champ.Nom, ref m_Nom, value);
					}
				}
			}
		}

		
		#endregion

		#region Constructeur
		/// <summary>
		/// Constructeur par défaut
		/// </summary>
		public Role()
            : base()
        {
			m_Nom = string.Empty;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Id">Identifiant de ce role</param>
		/// <param name="Nom">Nom de ce role</param>
		public Role(int Id, string Nom)
            : this()
        {
			DefinirId(Id);
			this.Nom = Nom;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
		public Role(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
			base.Connexion = Connexion;
			if (Enregistrement != null)
			{
				DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "id"));
				this.Nom = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "nom");
			}
		}

		#endregion

		/// <summary>
		/// Permet d'énumérer les entités correspondant aux enregistrements énumérés
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
		/// <returns>Enumération des entités issues des enregistrements énumérés</returns>
		public static IEnumerable<Role> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
		{
			return Enumerer(Enregistrements, Enregistrement => new Role(Connexion, Enregistrement));
		}

		
		#region Méthodes relatives au gestionnaire d'entités pour base de données MySQL
		/// <summary>
		/// Méthode retournant le nom de la table principale de ce type d'entités
		/// </summary>
		/// <returns>Nom de la table principale de ce type d'entités</returns>
		public override string NomDeLaTablePrincipale
		{
			get
			{
				return "role";
			}
		}

		/// <summary>
		/// Clause d'assignation utilisable dans une requête INSERT/UPDATE
		/// </summary>
		public override PDSGBD.MyDB.CodeSql ClauseAssignation
		{
			get
			{
				return new PDSGBD.MyDB.CodeSql("nom = {0}", m_Nom);
			}
		}
		
		#endregion

	}
}
