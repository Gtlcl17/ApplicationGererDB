using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace PDSGBD
{
    /// <summary>
    /// Permet de gérer une connexion à un serveur MySql afin d'y travailler sur une base de données
    /// </summary>
    public partial class MyDB : IDisposable
    {
        #region Constantes publiques
        /// <summary>
        /// Adresse d'accès par "défaut" au serveur MySql
        /// </summary>
        public const string AdresseServeurParDefaut = "localhost";

        /// <summary>
        /// Port par défaut des serveurs MySql
        /// </summary>
        public const int PortMySqlParDefaut = 3306;
        #endregion

        #region Sous-types publiques
        /// <summary>
        /// Etat de connexion
        /// </summary>
        public enum EtatConnexion
        {
            /// <summary>
            /// La connexion n'a pas encore été établie
            /// </summary>
            NonConnecte,
            /// <summary>
            /// La connexion était établie, mais a été perdue
            /// </summary>
            Perdu,
            /// <summary>
            /// La connexion avait été établie, mais a été volontairement fermée
            /// </summary>
            Ferme,
            /// <summary>
            /// La connexion a été établie, et à priori, l'est encore
            /// </summary>
            Connecte
        }

        /// <summary>
        /// Méthode exécutant des requêtes SQL
        /// </summary>
        public enum MethodeExecutantRequeteSql
        {
            Enumerer,
            Consulter,
            ValeurDe,
            ValeurDe_Typee,
            Executer
        }

        /// <summary>
        /// Type de méthode réagissant à l'événement SurChangementEtatConnexion
        /// </summary>
        /// <param name="ConnexionEmettrice">Objet de connexion MySql ayant déclenché cet événement</param>
        /// <param name="EtatPrecedent">Etat de connexion avant le changement</param>
        /// <param name="NouvelEtat">Nouvel état de connexion après le changement</param>
        public delegate void MethodeSurChangementEtatConnexion(MyDB ConnexionEmettrice, EtatConnexion EtatPrecedent, EtatConnexion NouvelEtat);

        /// <summary>
        /// Type de méthode réagissant à l'événement SurErreur
        /// </summary>
        /// <param name="ConnexionEmettrice">Objet de connexion MySql ayant déclenché cet événement</param>
        /// <param name="MethodeEmettrice">Méthode ayant produit cette erreur SQL</param>
        /// <param name="RequeteSql">Requête SQL ayant généré l'erreur</param>
        /// <param name="Valeurs">Valeurs des parties variables de cette requête SQL</param>
        /// <param name="MessageErreur">Message descriptif de cette erreur</param>
        public delegate void MethodeSurErreurSql(MyDB ConnexionEmettrice, MethodeExecutantRequeteSql MethodeEmettrice, string RequeteSql, object[] Valeurs, string MessageErreur);
        #endregion

        #region Membres privés gérant les paramètres avec la connexion au serveur MySql
        /// <summary>
        /// Nom de l'utilisateur MySql utilisé pour réaliser cette connexion à un serveur MySql
        /// </summary>
        private string m_Utilisateur;

        /// <summary>
        /// Mot de passe de l'utilisateur MySql utilisé pour réaliser cette connexion à un serveur MySql
        /// </summary>
        private string m_MotDePasse;

        /// <summary>
        /// Nom de la base de données à manipuler via cette connexion à un serveur MySql
        /// </summary>
        private string m_BaseDeDonnees;

        /// <summary>
        /// Adresse du serveur MySql auquel cette connexion se réfère
        /// </summary>
        private string m_AdresseServeur;

        /// <summary>
        /// Port du communication du serveur MySql auquel cette connexion se réfère
        /// </summary>
        private int m_PortMySql;

        /// <summary>
        /// Objet du .Net Connector permettant de gérer une connexion au serveur MySql
        /// </summary>
        private MySqlConnection m_Connexion;

        /// <summary>
        /// Stocke le dernier état connu de la connexion
        /// </summary>
        private EtatConnexion m_EtatConnu;
        #endregion

        #region Méthodes privées de gestion de la connexion
        /// <summary>
        /// Permet de définir les paramètres de connexion au serveur MySql pour travailler sur la base de données spécifiées
        /// </summary>
        /// <param name="Utilisateur">Nom de l'utilisateur MySql</param>
        /// <param name="MotDePasse">Mot de passe de l'utilisateur MySql</param>
        /// <param name="BaseDeDonnees">Nom de la base de données MySql</param>
        /// <param name="AdresseServeur">Adresse de machine du serveur MySql</param>
        /// <param name="PortMySql">Port de communication du serveur MySql</param>
        /// <returns>Vrai si les paramètres de connexion ont pu être définis avec les valeurs passées en paramètre, sinon faux</returns>
        private bool DefinirParametresConnexion(string Utilisateur, string MotDePasse, string BaseDeDonnees, string AdresseServeur = AdresseServeurParDefaut, int PortMySql = PortMySqlParDefaut)
        {
            if (string.IsNullOrWhiteSpace(Utilisateur) || string.IsNullOrWhiteSpace(MotDePasse) || string.IsNullOrWhiteSpace(BaseDeDonnees) || string.IsNullOrWhiteSpace(AdresseServeur) || (PortMySql <= 1024) || (PortMySql > 32767))
            {
                return false;
            }
            m_Utilisateur = Utilisateur;
            m_MotDePasse = MotDePasse;
            m_BaseDeDonnees = BaseDeDonnees;
            m_AdresseServeur = AdresseServeur;
            m_PortMySql = PortMySql;
            return true;
        }
        #endregion

        #region Constructeurs et destructeur
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public MyDB()
        {
            m_Utilisateur = null;
            m_MotDePasse = null;
            m_BaseDeDonnees = null;
            m_AdresseServeur = null;
            m_PortMySql = -1;
            m_Connexion = null;
            m_EtatConnu = EtatConnexion.NonConnecte;
        }

        /// <summary>
        /// Constructeur par copie
        /// <para>Si l'objet de gestion source est connecté, ce nouvel objet sera également connecté si possible</para>
        /// </summary>
        /// <param name="Source">Objet de gestion de connexion sur lequel on se base pour créer celui-ci</param>
        public MyDB(MyDB Source)
            : this()
        {
            m_Utilisateur = Source.m_Utilisateur;
            m_MotDePasse = Source.m_MotDePasse;
            m_BaseDeDonnees = Source.m_BaseDeDonnees;
            m_AdresseServeur = Source.m_AdresseServeur;
            m_PortMySql = Source.m_PortMySql;
            if (Source.EstConnecte) this.SeConnecter();
        }

        /// <summary>
        /// Constructeur permettant de définir les paramètres de connexion
        /// </summary>
        /// <param name="Utilisateur">Nom de l'utilisateur MySql</param>
        /// <param name="MotDePasse">Mot de passe de l'utilisateur MySql</param>
        /// <param name="BaseDeDonnees">Nom de la base de données MySql</param>
        /// <param name="AdresseServeur">Adresse de machine du serveur MySql</param>
        /// <param name="PortMySql">Port de communication du serveur MySql</param>
        public MyDB(string Utilisateur, string MotDePasse, string BaseDeDonnees, string AdresseServeur = AdresseServeurParDefaut, int PortMySql = PortMySqlParDefaut)
            : this()
        {
            DefinirParametresConnexion(Utilisateur, MotDePasse, BaseDeDonnees, AdresseServeur, PortMySql);
        }

        /// <summary>
        /// Permet de libérer les ressources associées à la connexion au serveur MySql
        /// </summary>
        public void Dispose()
        {
            SeDeconnecter();
        }

        /// <summary>
        /// Destructeur
        /// </summary>
        ~MyDB()
        {
            Dispose();
        }

        /// <summary>
        /// Permet de libérer toutes les ressources associées à cette connexion
        /// </summary>
        private void LibererConnexion()
        {
            if (m_Connexion != null)
            {
                if (m_Lecteur != null)
                {
                    m_Lecteur.Dispose();
                    m_Lecteur = null;
                }
                m_Connexion.Dispose();
                m_Connexion = null;
            }
        }
        #endregion

        #region Méthodes et propriétés publiques relatives à la connexion
        /// <summary>
        /// Tente de se connecter au serveur MySql avec les paramètres de connexion spécifiés
        /// <para>Si les paramètres de connexion semblent valides, ils sont enregistrés dans cet objet</para>
        /// </summary>
        /// <param name="Utilisateur">Nom de l'utilisateur MySql</param>
        /// <param name="MotDePasse">Mot de passe de l'utilisateur MySql</param>
        /// <param name="BaseDeDonnees">Nom de la base de données MySql</param>
        /// <param name="AdresseServeur">Adresse de machine du serveur MySql</param>
        /// <param name="PortMySql">Port de communication du serveur MySql</param>
        /// <returns>Vrai si la connexion a pu être établie, sinon faux</returns>
        public bool SeConnecter(string Utilisateur, string MotDePasse, string BaseDeDonnees, string AdresseServeur = AdresseServeurParDefaut, int PortMySql = PortMySqlParDefaut)
        {
            if (!DefinirParametresConnexion(Utilisateur, MotDePasse, BaseDeDonnees, AdresseServeur, PortMySql)) return false;
            return SeConnecter();
        }

        /// <summary>
        /// Tente de se connecter au serveur MySql avec les paramètres de connexion enregistrés
        /// </summary>
        /// <returns>Vrai si la connexion a pu être établie, sinon faux</returns>
        public bool SeConnecter()
        {
            if (m_Connexion != null) return false;
            try
            {
                m_Connexion = new MySqlConnection(string.Format("server={0};port={1};user={2};password={3};database={4};",
                    m_AdresseServeur,
                    m_PortMySql,
                    m_Utilisateur,
                    m_MotDePasse,
                    m_BaseDeDonnees));
                m_Connexion.Open();
                MySqlCommand Commande = new MySqlCommand();
                Commande.Connection = m_Connexion;
                Commande.CommandText = "SET NAMES 'latin1';";
                Commande.ExecuteNonQuery();
                Commande.CommandText = "SET character_set_results = 'latin1';";
                Commande.ExecuteNonQuery();
                EtatConnexionConnu = EtatConnexion.Connecte;
                return true;
            }
            catch (Exception Erreur)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(
                    "\nMyDB.SeConnecter({0}, {1}, {2}, {3}, {4}) a échoué :\n{5}\n",
                    m_Utilisateur,
                    !string.IsNullOrEmpty(m_MotDePasse) ? new string('*', m_MotDePasse.Length) : string.Empty,
                    m_BaseDeDonnees,
                    m_AdresseServeur,
                    m_PortMySql,
                    Erreur.Message));
                m_Connexion = null;
                return false;
            }
        }

        /// <summary>
        /// Permet de se déconnecter du serveur MySql
        /// </summary>
        /// <returns>Vrai si la déconnexion a été réalisée, sinon faux</returns>
        public bool SeDeconnecter()
        {
            LibererConnexion();
            EtatConnexionConnu = EtatConnexion.Ferme;
            return true;
        }

        /// <summary>
        /// Evénement déclenché en cas de changement d'état de la connexion
        /// </summary>
        public event MethodeSurChangementEtatConnexion SurChangementEtatConnexion = null;

        /// <summary>
        /// Indique si la connexion est établie (ou pas)
        /// </summary>
        public bool EstConnecte
        {
            /// <summary>
            /// Accesseur de cette propriété
            /// </summary>
            get
            {
                return (m_Connexion != null);
            }
        }

        /// <summary>
        /// Etat actuel de la connexion
        /// </summary>
        public EtatConnexion EtatConnexionConnu
        {
            get
            {
                return m_EtatConnu;
            }
            private set
            {
                if (value != m_EtatConnu)
                {
                    EtatConnexion EtatConnuPrecedent = m_EtatConnu;
                    m_EtatConnu = value;
                    if (SurChangementEtatConnexion != null)
                    {
                        SurChangementEtatConnexion(this, EtatConnuPrecedent, m_EtatConnu);
                    }
                }
            }
        }

        /// <summary>
        /// Permet de vérifier l'état de la connexion
        /// </summary>
        /// <returns>Vrai si la connexion est actuellement fonctionnelle, sinon faux</returns>
        public bool TesterConnexion()
        {
            if (m_Connexion == null) return false;
            bool ResultatPing = false;
            if (m_Lecteur == null)
            {
                ResultatPing = m_Connexion.Ping();
            }
            else
            {
                using (MyDB Connexion = new MyDB(this))
                {
                    ResultatPing = Connexion.m_Connexion.Ping();
                }
            }
            if (!ResultatPing)
            {
                LibererConnexion();
                EtatConnexionConnu = EtatConnexion.Perdu;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Evénement déclenché en cas d'erreur d'exécution d'une requête Sql
        /// </summary>
        public event MethodeSurErreurSql SurErreur = null;
        #endregion
    }
}
