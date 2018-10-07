using System;
using System.Collections;
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
        #region Membres et méthodes privés gérant la lecture d'enregistrements provenant du serveur MySql
        /// <summary>
        /// Objet permettant de consulter des enregistrements obtenus par l'exploitation de l'exécution d'une requête SELECT/SHOW
        /// <para>Un seul objet de lecture est autorisé par objet de connexion au serveur MySql !</para>
        /// <para>Quand un tel objet de consultation (MySqlDataReader) est en cours d'utilisation, l'objet de connexion ne permet plus aucun autre "dialogue" avec le serveur (aucune exécution d'autre requête n'est autorisée) !</para>
        /// </summary>
        private MySqlDataReader m_Lecteur;

        /// <summary>
        /// Indique si une lecture d'enregistrement est actuellement en cours
        /// </summary>
        private bool EstEnCoursDeLecture
        {
            get
            {
                return (m_Lecteur != null);
            }
        }

        /// <summary>
        /// Permet de débuter une lecture à partir de cette connexion si possible, sinon d'une nouvelle
        /// </summary>
        /// <param name="Commande">Commande de lecture à exécuter</param>
        /// <param name="ConnexionUtilisee">Connexion utilisée pour obtenir les enregistrements de cette lecture requise</param>
        /// <returns>Vrai si une connexion est disponible pour la lecture d'enregistrements, sinon faux</returns>
        private bool DebuterLecture(MySqlCommand Commande, out MyDB ConnexionUtilisee)
        {
            if (m_Lecteur == null)
            {
                try
                {
                    m_Lecteur = Commande.ExecuteReader();
                    ConnexionUtilisee = this;
                    return true;
                }
                catch /*(Exception Erreur)*/
                {
                    /*
                    System.Diagnostics.Debug.WriteLine(string.Format(
                        "\nMyDB.DebuterLecture({0}) a échoué :\n{1}\n",
                        Commande.CommandText,
                        Erreur.Message));
                    */
                    if (m_Lecteur != null)
                    {
                        m_Lecteur.Dispose();
                        m_Lecteur = null;
                    }
                    ConnexionUtilisee = null;
                    return false;
                }
            }
            else
            {
                MyDB AutreConnexion = new MyDB(this);
                if (!AutreConnexion.EstConnecte)
                {
                    ConnexionUtilisee = null;
                    return false;
                }
                Commande.Connection = AutreConnexion.m_Connexion;
                if (!AutreConnexion.DebuterLecture(Commande, out ConnexionUtilisee))
                {
                    AutreConnexion.Dispose();
                    ConnexionUtilisee = null;
                    return false;
                }
                ConnexionUtilisee = AutreConnexion;
                return true;
            }
        }

        /// <summary>
        /// Permet de terminer l'utilisation de l'objet de lecture, soit avec l'objet de connexion spécifié, soit avec cet objet de connexion
        /// </summary>
        /// <param name="Source">Objet de connexion ayant été utilisé pour la lecture</param>
        /// <returns>Vrai si la libération a pu être réalisée, sinon faux</returns>
        private bool TerminerLecture(MyDB Source)
        {
            if (this != Source)
            {
                this.Dispose();
                return true;
            }
            else
            {
                if (m_Lecteur == null) return false;
                m_Lecteur.Dispose();
                m_Lecteur = null;
                return true;
            }
        }
        #endregion

        #region Sous-types de gestion d'un enregistrement consulté
        /// <summary>
        /// Fournit les fonctionnalités de consultation d'un enregistrement produit par une consultation via une requête SELECT/SHOW
        /// </summary>
        public interface IEnregistrement : IEnumerable<object>
        {
            /// <summary>
            /// Indice de cet enregistrement
            /// </summary>
            int IndiceEnregistrement
            {
                get;
            }

            /// <summary>
            /// Nombre de champs de cet enregistrement
            /// </summary>
            int NombreChamps
            {
                get;
            }

            /// <summary>
            /// Enumération des noms des champs
            /// </summary>
            IEnumerable<string> NomsChamps
            {
                get;
            }

            /// <summary>
            /// Retourne le nom du champ spécifié par un indice
            /// </summary>
            /// <param name="IndiceChamp">Indice du champ pour lequel on veut récupérer son nom</param>
            /// <returns>Nom du champ si il existe, sinon null</returns>
            string NomChamp(int IndiceChamp);

            /// <summary>
            /// Retourne la valeur typée du champ spécifié par un indice
            /// </summary>
            /// <typeparam name="T">Type de la valeur attendue</typeparam>
            /// <param name="IndiceChamp">Indice du champ pour lequel on veut récupérer sa valeur</param>
            /// <param name="ValeurParDefaut">Valeur par défaut à retourner si le champ n'existe pas ou si sa valeur n'est pas du type attendu</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            T ValeurChamp<T>(int IndiceChamp, T ValeurParDefaut = default(T));

            /// <summary>
            /// Retourne la valeur typée du champ spécifié par un nom
            /// </summary>
            /// <typeparam name="T">Type de la valeur attendue</typeparam>
            /// <param name="NomChamp">Nom du champ pour lequel on veut récupérer sa valeur</param>
            /// <param name="ValeurParDefaut">Valeur par défaut à retourner si le champ n'existe pas ou si sa valeur n'est pas du type attendu</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            T ValeurChamp<T>(string NomChamp, T ValeurParDefaut = default(T));

            /// <summary>
            /// Retourne la valeur typée du champ spécifié par un nom
            /// </summary>
            /// <typeparam name="T">Type de la valeur attendue</typeparam>
            /// <param name="NomTable">Nom de la table dont est issu ce champ</param>
            /// <param name="NomChamp">Nom du champ pour lequel on veut récupérer sa valeur</param>
            /// <param name="ValeurParDefaut">Valeur par défaut à retourner si le champ n'existe pas ou si sa valeur n'est pas du type attendu</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            T ValeurChampComplet<T>(string NomTable, string NomChamp, T ValeurParDefaut = default(T));

            /// <summary>
            /// Indexeur de la valeur non typée du champ spécifié par un indice
            /// </summary>
            /// <param name="IndiceChamp">Indice du champ pour lequel on veut récupérer sa valeur</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            object this[int IndiceChamp]
            {
                get;
            }

            /// <summary>
            /// Indexeur de la valeur non typée du champ spécifié par un nom
            /// </summary>
            /// <param name="NomChamp">Nom du champ pour lequel on veut récupérer sa valeur</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            object this[string NomChamp]
            {
                get;
            }
        }

        /// <summary>
        /// Permet de représenter un enregistrement produit par une consultation via une requête SELECT/SHOW
        /// <para>Réservé à un usage interne des méthodes de consultation d'enregistrement résultant d'une requête SELECT/SHOW</para> 
        /// <para>Exposable publiquement via l'interface publique IEnregistrement</para>
        /// </summary>
        private class Enregistrement : IEnregistrement
        {
            #region Membres privés
            /// <summary>
            /// Tableau vide à utiliser comme retour d'énumération sur un enregistrement non défini
            /// </summary>
            private static readonly object[] c_AucunChamp = new object[0];

            /// <summary>
            /// Membre stockant l'indice de cet enregistrement
            /// </summary>
            private int m_IndiceEnregistrement;

            /// <summary>
            /// Tableau stockant les noms des champs
            /// </summary>
            private string[] m_NomsChamps;

            /// <summary>
            /// Tableau stockant les valeurs des champs
            /// </summary>
            private object[] m_ValeursChamps;
            #endregion

            #region Implémentation de l'interface IEnregistrement
            public IEnumerator<object> GetEnumerator()
            {
                return ((m_ValeursChamps != null) ? m_ValeursChamps : c_AucunChamp).Cast<object>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((m_ValeursChamps != null) ? m_ValeursChamps : c_AucunChamp).GetEnumerator();
            }

            /// <summary>
            /// Indice de cet enregistrement
            /// </summary>
            public int IndiceEnregistrement
            {
                get
                {
                    return m_IndiceEnregistrement;
                }
            }

            /// <summary>
            /// Nombre de champs de cet enregistrement
            /// </summary>
            public int NombreChamps
            {
                get
                {
                    return m_NomsChamps.Length;
                }
            }

            /// <summary>
            /// Enumération des noms des champs
            /// </summary>
            public IEnumerable<string> NomsChamps
            {
                get
                {
                    return m_NomsChamps;
                }
            }

            /// <summary>
            /// Retourne le nom du champ spécifié par un indice
            /// </summary>
            /// <param name="IndiceChamp">Indice du champ pour lequel on veut récupérer son nom</param>
            /// <returns>Nom du champ si il existe, sinon null</returns>
            public string NomChamp(int IndiceChamp)
            {
                return ((IndiceChamp >= 0) && (IndiceChamp < m_NomsChamps.Length)) ? m_NomsChamps[IndiceChamp] : null;
            }

            /// <summary>
            /// Retourne la valeur typée du champ spécifié par un indice
            /// </summary>
            /// <typeparam name="T">Type de la valeur attendue</typeparam>
            /// <param name="IndiceChamp">Indice du champ pour lequel on veut récupérer sa valeur</param>
            /// <param name="ValeurParDefaut">Valeur par défaut à retourner si le champ n'existe pas ou si sa valeur n'est pas du type attendu</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            public T ValeurChamp<T>(int IndiceChamp, T ValeurParDefaut = default(T))
            {
                object Resultat = this[IndiceChamp];
                return (Resultat is T) ? (T)Resultat : ValeurParDefaut;
            }

            /// <summary>
            /// Retourne la valeur typée du champ spécifié par un nom
            /// </summary>
            /// <typeparam name="T">Type de la valeur attendue</typeparam>
            /// <param name="NomChamp">Nom du champ pour lequel on veut récupérer sa valeur</param>
            /// <param name="ValeurParDefaut">Valeur par défaut à retourner si le champ n'existe pas ou si sa valeur n'est pas du type attendu</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            public T ValeurChamp<T>(string NomChamp, T ValeurParDefaut = default(T))
            {
                object Resultat = this[NomChamp];
                if (Resultat == null) return ValeurParDefaut;
                if (Resultat is MySql.Data.Types.MySqlDateTime)
                {
                    MySql.Data.Types.MySqlDateTime Valeur = (MySql.Data.Types.MySqlDateTime)Resultat;
                    Resultat = Valeur.IsValidDateTime ? (object)Valeur.GetDateTime() : ValeurParDefaut;
                }
                else if (Resultat is MySql.Data.Types.MySqlDecimal)
                {
                    MySql.Data.Types.MySqlDecimal Valeur = (MySql.Data.Types.MySqlDecimal)Resultat;
                    Resultat = !Valeur.IsNull ? (object)Valeur.Value : ValeurParDefaut;
                }
                return (Resultat is T) ? (T)Resultat : ValeurParDefaut;
            }

            /// <summary>
            /// Retourne la valeur typée du champ spécifié par un nom (et un nom préfixe de table)
            /// </summary>
            /// <typeparam name="T">Type de la valeur attendue</typeparam>
            /// <param name="NomTable">Nom de la table dont est issu ce champ</param>
            /// <param name="NomChamp">Nom du champ pour lequel on veut récupérer sa valeur</param>
            /// <param name="ValeurParDefaut">Valeur par défaut à retourner si le champ n'existe pas ou si sa valeur n'est pas du type attendu</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            public T ValeurChampComplet<T>(string NomTable, string NomChamp, T ValeurParDefaut = default(T))
            {
                string NomPartiel = NomChamp;
                string NomComplet = string.Format("{0}.{1}", NomTable, NomPartiel);
                return ValeurChamp<T>(m_NomsChamps.Contains(NomComplet) ? NomComplet : NomPartiel, ValeurParDefaut);
            }

            /// <summary>
            /// Indexeur de la valeur non typée du champ spécifié par un indice
            /// </summary>
            /// <param name="IndiceChamp">Indice du champ pour lequel on veut récupérer sa valeur</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            public object this[int IndiceChamp]
            {
                get
                {
                    return ((IndiceChamp >= 0) && (IndiceChamp < m_ValeursChamps.Length)) ? m_ValeursChamps[IndiceChamp] : null;
                }
            }

            /// <summary>
            /// Indexeur de la valeur non typée du champ spécifié par un nom
            /// </summary>
            /// <param name="NomChamp">Nom du champ pour lequel on veut récupérer sa valeur</param>
            /// <returns>Valeur du champ si il existe, sinon null</returns>
            public object this[string NomChamp]
            {
                get
                {
                    if (string.IsNullOrEmpty(NomChamp)) return null;
                    for (int IndiceChamp = 0; IndiceChamp < m_NomsChamps.Length; IndiceChamp++)
                    {
                        if (m_NomsChamps[IndiceChamp] == NomChamp) return m_ValeursChamps[IndiceChamp];
                    }
                    return null;
                }
            }
            #endregion

            /// <summary>
            /// Constructeur
            /// </summary>
            /// <param name="Lecteur">Objet MySqlDataReader servant à la récupération des noms de champs</param>
            public Enregistrement(MySqlDataReader Lecteur)
            {
                m_IndiceEnregistrement = -1;
                m_NomsChamps = new string[Lecteur.FieldCount];
                m_ValeursChamps = new object[m_NomsChamps.Length];
                for (int Indice = 0; Indice < Lecteur.FieldCount; Indice++)
                {
                    m_NomsChamps[Indice] = Lecteur.GetName(Indice);
                    m_ValeursChamps[Indice] = null;
                }
            }

            /// <summary>
            /// Permet de transférer dans cet objet représentant un enregistrement, les valeurs des champs de l'enregistrement consultable par l'objet MySqlDataReader spécifié
            /// </summary>
            /// <param name="Lecteur">Objet MySqlDataReader servant à la consultation des valeurs de champs de l'enregistrement courant</param>
            /// <returns>Vrai si le chargement a pu se faire, sinon faux</returns>
            public bool ChargerValeurs(int IndiceEnregistrement, MySqlDataReader Lecteur)
            {
                m_IndiceEnregistrement = IndiceEnregistrement;
                try
                {
                    for (int Indice = 0; Indice < m_ValeursChamps.Length; Indice++)
                    {
                        m_ValeursChamps[Indice] = Lecteur.GetValue(Indice);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region Méthodes publiques de consultation via des requêtes SELECT ou SHOW
        /// <summary>
        /// Permet de récupérer la valeur typée du premier champ du premier enregistrement résultant d'une requête de consultation
        /// </summary>
        /// <typeparam name="T">Type de la valeur attendue</typeparam>
        /// <param name="ValeurParDefaut">Valeur par défaut à retourner si la requête n'a pas pu fournir d'enregistrement ou si sa valeur n'est pas du type attendu</param>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type SELECT ou SHOW)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Valeur typée du premier champ du premier enregistrement si possible, sinon null</returns>
        public T ValeurDeAvecDefaut<T>(T ValeurParDefaut, string Requete, params object[] Valeurs)
        {
            object Resultat = ValeurDeAvecDefaut(ValeurParDefaut, Requete, Valeurs);
            if (Resultat is T) return (T)Resultat;
            if (SurErreur != null)
            {
                SurErreur(this, MethodeExecutantRequeteSql.ValeurDe_Typee, Requete, Valeurs, string.Format("Imcompatibilité entre le résultat qui est de type {0} et le type attendu qui est {1} !", (Resultat != null) ? Resultat.GetType().FullName : "null", typeof(T).FullName));
            }
            return ValeurParDefaut;
        }

        /// <summary>
        /// Permet de récupérer la valeur typée du premier champ du premier enregistrement résultant d'une requête de consultation
        /// </summary>
        /// <typeparam name="T">Type de la valeur attendue</typeparam>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type SELECT ou SHOW)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Valeur typée du premier champ du premier enregistrement si possible, sinon null</returns>
        public T ValeurDe<T>(string Requete, params object[] Valeurs)
        {
            object Resultat = ValeurDeAvecDefaut(default(T), Requete, Valeurs);
            if (Resultat is T) return (T)Resultat;
            if (SurErreur != null)
            {
                SurErreur(this, MethodeExecutantRequeteSql.ValeurDe_Typee, Requete, Valeurs, string.Format("Imcompatibilité entre le résultat qui est de type {0} et le type attendu qui est {1} !", (Resultat != null) ? Resultat.GetType().FullName : "null", typeof(T).FullName));
            }
            return default(T);
        }

        /// <summary>
        /// Permet de récupérer la valeur non typée du premier champ du premier enregistrement résultant d'une requête de consultation
        /// </summary>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type SELECT ou SHOW)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Valeur non typée du premier champ du premier enregistrement si possible, sinon null</returns>
        public object ValeurDe(string Requete, params object[] Valeurs)
        {
            return ValeurDeAvecDefaut(null, Requete, Valeurs);
        }

        /// <summary>
        /// Permet de récupérer la valeur non typée du premier champ du premier enregistrement résultant d'une requête de consultation
        /// </summary>
        /// <param name="ValeurParDefaut">Valeur par défaut à retourner si la requête n'a pas pu fournir d'enregistrement</param>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type SELECT ou SHOW)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Valeur non typée du premier champ du premier enregistrement si possible, sinon la valeur par défaut spécifiée</returns>
        public object ValeurDeAvecDefaut(object ValeurParDefaut, string Requete, params object[] Valeurs)
        {
            for (int NombreEchecs = 0; NombreEchecs < 2; NombreEchecs++)
            {
                MyDB ConnexionUtilisee = null;
                try
                {
                    // Vérification de l'état théorique de la connexion
                    if (!EstConnecte) throw new Exception("Pas de connexion établie au serveur MySql");
                    // Vérification qu'il s'agit bien d'une requête de consultation
                    if (!EstRequeteConsultation(Requete)) throw new Exception("Une requête d'action ne peut pas faire l'objet de la méthode MyDB.Consulter()");
                    // Création de la commande SQL
                    MySqlCommand Commande = CreerCommande(Requete, Valeurs);
                    if (!DebuterLecture(Commande, out ConnexionUtilisee)) throw new Exception("Aucun objet de lecture n'a pu être créé");
                    // Tentative de lecture du premier enregistrement
                    if (ConnexionUtilisee.m_Lecteur.Read())
                    {
                        object Valeur = ConnexionUtilisee.m_Lecteur.GetValue(0);
                        // Terminaison de la lecture avec libération des ressources associées
                        if (ConnexionUtilisee != null) ConnexionUtilisee.TerminerLecture(this);
                        // Retour de la valeur du premier champ du premier enregistrement lu
                        return Valeur;
                    }
                    if (ConnexionUtilisee != null) ConnexionUtilisee.TerminerLecture(this);
                }
                catch (Exception Erreur)
                {
                    /*
                    System.Diagnostics.Debug.WriteLine(string.Format(
                        "\nMyDB.ValeurDe({0}, {1}) a échoué :\n{2}\n",
                        Requete,
                        string.Join("", Valeurs.Select(Valeur => string.Format(", {0}", Valeur))),
                        Erreur.Message));
                    */
                    if (ConnexionUtilisee != null) ConnexionUtilisee.TerminerLecture(this);
                    // Test de l'état de connexion
                    if ((EtatConnexionConnu != EtatConnexion.Perdu) && (!EstConnecte || TesterConnexion()))
                    {
                        if (SurErreur != null)
                        {
                            SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                        }
                        break;
                    }
                    // En cas de perte de connexion, on tente une fois de se reconnecter
                    if (!SeConnecter())
                    {
                        if (SurErreur != null)
                        {
                            SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                        }
                        break;
                    }
                    // En cas de réussite de la re-connexion, on boucle une seconde fois sur la tentative d'exécution de cette lecture
                }
            }
            // Retour d'une valeur par défaut afin de représenter l'absence d'enregistrement
            return ValeurParDefaut;
        }

        /// <summary>
        /// Permet de récupérer le premier enregistrement résultant d'une requête de consultation
        /// </summary>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type SELECT ou SHOW)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Enregistrement si possible, sinon null</returns>
        public IEnregistrement Consulter(string Requete, params object[] Valeurs)
        {
            for (int NombreEchecs = 0; NombreEchecs < 2; NombreEchecs++)
            {
                MyDB ConnexionUtilisee = null;
                try
                {
                    // Vérification de l'état théorique de la connexion
                    if (!EstConnecte) throw new Exception("Pas de connexion établie au serveur MySql");
                    // Vérification qu'il s'agit bien d'une requête de consultation
                    if (!EstRequeteConsultation(Requete)) throw new Exception("Une requête d'action ne peut pas faire l'objet de la méthode MyDB.Consulter()");
                    // Création de la commande SQL
                    MySqlCommand Commande = CreerCommande(Requete, Valeurs);
                    if (!DebuterLecture(Commande, out ConnexionUtilisee)) throw new Exception("Aucun objet de lecture n'a pu être créé");
                    // Préparation de l'objet représentant l'enregistrement courant
                    Enregistrement Enregistrement = new Enregistrement(ConnexionUtilisee.m_Lecteur);
                    // Tentative de lecture du premier enregistrement
                    if (ConnexionUtilisee.m_Lecteur.Read())
                    {
                        // Transfert des valeurs de l'enregistrement courant dans le dictionnaire le représentant
                        Enregistrement.ChargerValeurs(0, ConnexionUtilisee.m_Lecteur);
                        // Terminaison de la lecture avec libération des ressources associées
                        if (ConnexionUtilisee != null) ConnexionUtilisee.TerminerLecture(this);
                        // Retour du premier enregistrement lu
                        return Enregistrement;
                    }
                    // Terminaison de la lecture avec libération des ressources associées
                    if (ConnexionUtilisee != null) ConnexionUtilisee.TerminerLecture(this);
                }
                catch (Exception Erreur)
                {
                    /*
                    System.Diagnostics.Debug.WriteLine(string.Format(
                        "\nMyDB.Consulter({0}, {1}) a échoué (version de récupération du premier enregistrement) :\n{2}\n",
                        Requete,
                        string.Join("", Valeurs.Select(Valeur => string.Format(", {0}", Valeur))),
                        Erreur.Message));
                    */
                    // Terminaison de la lecture avec libération des ressources associées
                    if (ConnexionUtilisee != null) ConnexionUtilisee.TerminerLecture(this);
                    // Test de l'état de connexion
                    if ((EtatConnexionConnu != EtatConnexion.Perdu) && (!EstConnecte || TesterConnexion()))
                    {
                        if (SurErreur != null)
                        {
                            SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                        }
                        break;
                    }
                    // En cas de perte de connexion, on tente une fois de se reconnecter
                    if (!SeConnecter())
                    {
                        if (SurErreur != null)
                        {
                            SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                        }
                        break;
                    }
                    // En cas de réussite de la re-connexion, on boucle une seconde fois sur la tentative d'exécution de cette lecture
                }
            }
            // Retour d'un enregistrement "null" afin de représenter l'absence d'enregistrement
            return null;
        }

        /// <summary>
        /// Permet d'énumérer les enregistrements résultant d'une requête de consultation par une méthode passée en paramètre
        /// </summary>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type SELECT ou SHOW)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Chaque enregistrement produit par cette requête de consultation</returns>
        public IEnumerable<IEnregistrement> Enumerer(string Requete, params object[] Valeurs)
        {
            using (GestionnaireDeConnexionDeLecture GestionnaireConnexionLecture = new GestionnaireDeConnexionDeLecture(this))
            {
                // Boucle permettant de réessayer après une reconnexion
                for (int NombreEchecs = 0; NombreEchecs < 2; NombreEchecs++)
                {
                    Enregistrement Enregistrement = null;
                    int IndiceEnregistrement = 0;
                    while (true)
                    {
                        try
                        {
                            if (Enregistrement == null)
                            {
                                // Vérification de l'état théorique de la connexion
                                if (!EstConnecte) throw new Exception("Pas de connexion établie au serveur MySql");
                                // Vérification qu'il s'agit bien d'une requête de consultation
                                if (!EstRequeteConsultation(Requete)) throw new Exception("Une requête d'action ne peut pas faire l'objet de la méthode MyDB.Consulter()");
                                // Création de la commande SQL
                                MySqlCommand Commande = CreerCommande(Requete, Valeurs);
                                if (!GestionnaireConnexionLecture.DebuterLecture(Commande)) throw new Exception("Aucun objet de lecture n'a pu être créé");
                                // Préparation de l'objet représentant l'enregistrement courant
                                Enregistrement = new Enregistrement(GestionnaireConnexionLecture.ConnexionUtilisee.m_Lecteur);
                            }
                            // Boucle de lecture des enregistrements
                            if (!GestionnaireConnexionLecture.ConnexionUtilisee.m_Lecteur.Read())
                            {
                                /*
                                // Terminaison de la lecture avec libération des ressources associées
                                if (ConnexionLecture.ConnexionUtilisee != null) ConnexionLecture.ConnexionUtilisee.TerminerLecture(this);
                                */
                                // Permet de quitter la boucle de lecture des enregistrements et cette méthode d'énumération
                                yield break;
                            }
                            // Transfert des valeurs de l'enregistrement courant dans le dictionnaire le représentant
                            Enregistrement.ChargerValeurs(IndiceEnregistrement, GestionnaireConnexionLecture.ConnexionUtilisee.m_Lecteur);
                            // Incrémentation du nombre d'enregistrements consultés
                            IndiceEnregistrement++;
                        }
                        catch (Exception Erreur)
                        {
                            /*
                            System.Diagnostics.Debug.WriteLine(string.Format(
                                "\nMyDB.Consulter({0}, {1}) a échoué (version avec méthode de traitement) :\n{2}\n",
                                Requete,
                                string.Join("", Valeurs.Select(Valeur => string.Format(", {0}", Valeur))),
                                Erreur.Message));
                            */
                            if (SurErreur != null)
                            {
                                SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                            }
                            /*
                            // Terminaison de la lecture avec libération des ressources associées
                            if (ConnexionLecture.ConnexionUtilisee != null) ConnexionLecture.ConnexionUtilisee.TerminerLecture(this);
                            */
                            // Test de l'état de connexion
                            if ((EtatConnexionConnu != EtatConnexion.Perdu) && (!EstConnecte || TesterConnexion()))
                            {
                                if (SurErreur != null)
                                {
                                    SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                                }
                                yield break; // Permet de quitter la boucle de lecture des enregistrements et cette méthode d'énumération
                            }
                            // En cas de perte de connexion, on tente une fois de se reconnecter
                            if (!SeConnecter())
                            {
                                if (SurErreur != null)
                                {
                                    SurErreur(this, MethodeExecutantRequeteSql.Enumerer, Requete, Valeurs, Erreur.Message);
                                }
                                yield break; // Permet de quitter la boucle de lecture des enregistrements et cette méthode d'énumération
                            }
                            // En cas de réussite de la re-connexion, on boucle une seconde fois sur la tentative d'exécution de cette lecture
                            break; // Permet de quitter la boucle de lecture des enregistrements
                        }
                        // Enumération de l'élément courant (comme retour partiel devant être "consommé")
                        yield return Enregistrement;
                    }
                }
            }
        }
        #endregion

        #region Classe utilitaire pour l'énumérateur d'enregistrements produit par une requête de consultation
        /// <summary>
        /// Classe privée utilisée pour gérer la connexion utilisée par l'énumérateur d'enregistrements
        /// </summary>
        private class GestionnaireDeConnexionDeLecture : IDisposable
        {
            /// <summary>
            /// Connexion "maître"
            /// </summary>
            private MyDB m_ConnexionMaitre;

            /// <summary>
            /// Connexion utilisée pour la lecture
            /// </summary>
            private MyDB m_ConnexionUtilisee;

            /// <summary>
            /// Connexion utilisée pour la lecture
            /// </summary>
            public MyDB ConnexionUtilisee
            {
                get
                {
                    return m_ConnexionUtilisee;
                }
            }

            /// <summary>
            /// Constructeur
            /// </summary>
            /// <param name="ConnexionMaitre">Connection "maître"</param>
            public GestionnaireDeConnexionDeLecture(MyDB ConnexionMaitre)
            {
                m_ConnexionMaitre = ConnexionMaitre;
                m_ConnexionUtilisee = null;
            }

            /// <summary>
            /// Permet de débuter une lecture à partir de la connexion "maître" si possible, sinon d'une nouvelle
            /// </summary>
            /// <param name="Commande">Commande de lecture à exécuter</param>
            /// <returns>Vrai si une connexion est disponible pour la lecture d'enregistrements, sinon faux</returns>
            public bool DebuterLecture(MySqlCommand Commande)
            {
                return m_ConnexionMaitre.DebuterLecture(Commande, out m_ConnexionUtilisee);
            }

            /// <summary>
            /// Libère les ressources de lecture de la connexion utilisée
            /// </summary>
            public void Dispose()
            {
                if (m_ConnexionUtilisee != null)
                {
                    m_ConnexionUtilisee.TerminerLecture(m_ConnexionMaitre);
                }
            }
        }
        #endregion
    }
}
