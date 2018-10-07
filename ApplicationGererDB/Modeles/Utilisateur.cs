using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	public class Utilisateur : Entite<Utilisateur, Utilisateur.Champ>
	{
		/// <summary>
		/// Champ décrivant cet utilisateur
		/// </summary>
		public enum Champ
		{
			/// <summary>
			/// Identifiant de cet utilisateur
			/// </summary>
			Id,
			/// <summary>
			/// Nom de cet utilisateur
			/// </summary>
			Nom,
			/// <summary>
			/// email de cet utilisateur
			/// </summary>
			Email,

		}

		#region Membres privés
		/// <summary>
		/// Stocke la valeur du champ Nom
		/// </summary>
		private string m_Nom;

		/// <summary>
		/// Stocke la valeur du champ Email
		/// </summary>
		private string m_Email;

		#endregion

		#region Membres publics
		/// <summary>
		/// Nom de de l'utilisateur
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

		/// <summary>
		/// Email de l'utilisateur
		/// </summary>
		public string Email
		{
			get
			{
				return m_Email;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					Declencher_SurErreur(this, Champ.Nom, "Email vide ou ne contient que des espaces");
				}
				else
				{
					value = value.Trim();
					if (!string.Equals(value, m_Nom))
					{
						ModifierChamp(Champ.Email, ref m_Email, value);
					}
				}
			}
		}


		#endregion

		#region Constructeur
		/// <summary>
		/// Constructeur par défaut
		/// </summary>
		public Utilisateur()
            : base()
        {
			m_Nom = string.Empty;
			m_Email = string.Empty;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Id">Identifiant de cet utilisateur</param>
		/// <param name="Nom">Nom de cet utilisateur</param>
		/// <param name="Email">Email de cet utilisateur</param>
		public Utilisateur(int Id, string Nom, string Email)
            : this()
        {
			DefinirId(Id);
			this.Nom = Nom;
			this.Email = Email;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
		public Utilisateur(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
			base.Connexion = Connexion;
			if (Enregistrement != null)
			{
				DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "id"));
				this.Nom = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "nom");
				this.Email = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "email");
			}
		}

		#endregion

		/// <summary>
		/// Permet d'énumérer les entités correspondant aux enregistrements énumérés
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
		/// <returns>Enumération des entités issues des enregistrements énumérés</returns>
		public static IEnumerable<Utilisateur> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
		{
			return Enumerer(Enregistrements, Enregistrement => new Utilisateur(Connexion, Enregistrement));
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
				return "Utilisateur";
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
