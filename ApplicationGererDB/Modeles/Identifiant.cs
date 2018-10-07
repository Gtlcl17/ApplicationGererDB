using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
    public class Identifiant : Entite<Identifiant, Identifiant.Champ>
    {
       
        /// <summary>
        /// Champ décrivant cet identifiant
        /// </summary>
        public enum Champ
        {
            /// <summary>
            /// Identifiant de cet identifiant
            /// </summary>
            Id,
            /// <summary>
            /// Login de cet indentifiant
            /// </summary>
            Login,
            /// <summary>
            /// Mot de passe de cet identifiant
            /// </summary>
            Mdp,
            /// <summary>
            /// Référence de l'utilisateur lié à cet utilisateur
            /// </summary>
            Ref_utilisateur,
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
        /// Login de cet identifiant
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
        /// Mot de passe de cet identifiant
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
		/// Descriptif de l'utilisateur lié à cet identifiant
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
					Declencher_SurErreur(this, Champ.Ref_utilisateur, "Association d'utilisateur et d'identifiant indéfinie !");
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
		/// Descriptif du rôle de cet identifiant
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
                    Declencher_SurErreur(this, Champ.Ref_role, "Association de rôle et d'identifiant indéfinie !");
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
        public Identifiant()
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
        /// <param name="Id">Identifiant de cet identifiant</param>
        /// <param name="Login">Login de cet identifiant</param>
        /// <param name="Mdp">Mot de passe de cet identifiant</param>
        /// <param name="UtilisateurReference">Utilisateur référencé par cet identifiant</param>
        /// <param name="CategorieReferencee">Rôle référencé par cet identifiant</param>
        public Identifiant(int Id, string Login, string Mdp, Utilisateur UtilisateurReference, Role RoleReference)
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
        public Identifiant(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
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
        public static IEnumerable<Identifiant> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
        {
            return Enumerer(Enregistrements, Enregistrement => new Identifiant(Connexion, Enregistrement));
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
                return "identifiants";
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
        
        #endregion

    }
}
