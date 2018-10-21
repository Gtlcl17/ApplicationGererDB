using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace ApplicationGererDB
{
    public class Character : Entite<Character, Character.Champ>
    {
       
        /// <summary>
        /// Champ décrivant ce Character
        /// </summary>
        public enum Champ
        {
            /// <summary>
            /// Identifiant de ce Character
            /// </summary>
            Id,
            /// <summary>
            /// Login de cet indentifiant
            /// </summary>
            Nom,
            /// <summary>
            /// Mot de passe de cet Character
            /// </summary>
            Rank,
            /// <summary>
            /// Référence de l'utilisateur lié à cet utilisateur
            /// </summary>
            Cost,
            /// <summary>
            /// Référence du rôle de cet indentifiant
            /// </summary>
            Ref_role,
            
        }

        #region Membres privés
        /// <summary>
        /// Stocke la valeur du champ Titre
        /// </summary>
        private string m_Login;
        /// <summary>
        /// Stocke la valeur du champ Description
        /// </summary>
        private string m_Mdp;
		/// <summary>
		/// Stocke la référence de l'utilisateur
		/// </summary>
		private Utilisateur m_ref_Utilisateur;
        /// <summary>
        /// Stocke la référence du rôle
        /// </summary>
        private Role m_ref_role;
        
        #endregion

        #region Membres publics

        /// <summary>
        /// Login de cet Character
        /// </summary>
        public string Login
        {
            get
            {
                 return m_Login;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Declencher_SurErreur(this, Champ.Login, "Titre vide ou ne contient que des espaces");
                }
                else
                {
                    value = value.Trim();
                    if(!string.Equals(value, m_Login))
                    {
                        ModifierChamp(Champ.Login, ref m_Login, value);
                    }
                }
            }
        }

        /// <summary>
        /// Mot de passe de cet Character
        /// </summary>
        public string Mdp
        {
            get
            {
                return m_Mdp;
            }

            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    Declencher_SurErreur(this, Champ.Mdp, "Description vide ou ne contient que des espaces");
                }
                else
                {
                    value = value.Trim();
                    ModifierChamp(Champ.Mdp, ref m_Mdp, value);
                }
            }
        }

		/// <summary>
		/// Descriptif de l'utilisateur lié à cet Character
		/// </summary>
		public Utilisateur ref_Utilisateur
		{
			get
			{
				return m_ref_Utilisateur;
			}
			set
			{
				if (value == null)
				{
					Declencher_SurErreur(this, Champ.Ref_utilisateur, "Association d'utilisateur et d'Character indéfinie !");
				}
				else
				{
					if ((m_ref_Utilisateur == null) || !int.Equals(value.Id, m_ref_Utilisateur.Id))
					{
						ModifierChamp(Champ.Ref_utilisateur, ref m_ref_Utilisateur, value);
					}
				}
			}
		}


		/// <summary>
		/// Descriptif du rôle de cet Character
		/// </summary>
		public Role ref_Role
        {
            get
            {
                return m_ref_role;
            }
            set
            {
                if (value == null)
                {
                    Declencher_SurErreur(this, Champ.Ref_role, "Association de rôle et d'Character indéfinie !");
                }
                else
                {
                    if ((m_ref_role == null) || !int.Equals(value.Id, m_ref_role.Id))
                    {
                        ModifierChamp(Champ.Ref_role, ref m_ref_role, value);
                    }
                }
            }
        }

        
        #endregion
		        
        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Character()
            : base()
        {
            m_Login = string.Empty;
            m_Mdp = string.Empty;
            m_ref_Utilisateur = null;
			m_ref_role = null;
        }

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Id">Character de cet Character</param>
        /// <param name="Login">Login de cet Character</param>
        /// <param name="Mdp">Mot de passe de cet Character</param>
        /// <param name="UtilisateurReference">Utilisateur référencé par cet Character</param>
        /// <param name="CategorieReferencee">Rôle référencé par cet Character</param>
        public Character(int Id, string Login, string Mdp, Utilisateur UtilisateurReference, Role RoleReference)
            : this()
        {
            DefinirId(Id);
            this.Login = Login;
            this.Mdp = Mdp;
            this.ref_Utilisateur = UtilisateurReference;
            this.ref_Role = RoleReference;
        }

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        /// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
        public Character(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
            base.Connexion = Connexion;
            if (Enregistrement != null)
            {
                DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "id"));
                this.Login = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "titre");
                this.Mdp = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "description");
				this.m_ref_Utilisateur = new Utilisateur(Connexion, Enregistrement);
				this.m_ref_role = new Role(Connexion, Enregistrement);
            }
        }

        /// <summary>
        /// Permet d'énumérer les entités correspondant aux enregistrements énumérés
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        /// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
        /// <returns>Enumération des entités issues des enregistrements énumérés</returns>
        public static IEnumerable<Character> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
        {
            return Enumerer(Enregistrements, Enregistrement => new Character(Connexion, Enregistrement));
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
                return "Characters";
            }
        }

        /// <summary>
        /// Clause d'assignation utilisable dans une requête INSERT/UPDATE
        /// </summary>
        public override PDSGBD.MyDB.CodeSql ClauseAssignation
        {
            get
            {
                return new PDSGBD.MyDB.CodeSql("login = {0}, mdp = {1}, ref_utilisateur = {2}, ref_role = {3}", m_Login, m_Mdp, m_ref_Utilisateur.Id, m_ref_Utilisateur.Id);
            }
        }
        /*  A voir ce qu'on fait de ce delete ...
        /// <summary>
        /// Permet de supprimer tous les changements de status liés à ce projet
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        public override void SupprimerEnCascade(PDSGBD.MyDB Connexion)
        {
            Connexion.Executer("DELETE FROM hist_projet WHERE ref_projet = {0}", Id);
        }
        */
/*        
        #endregion

    }
}
*/