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
        #region Sous-types de gestion des résultats d'exécution de requête d'action
        /// <summary>
        /// Fournit les informations relatives aux résultats d'exécution d'une requête d'action
        /// </summary>
        public interface IResultatExecution
        {
            /// <summary>
            /// Indicateur de réussite ou d'échec
            /// </summary>
            bool Reussite { get; }

            /// <summary>
            /// Nombre d'enregistrements affectés par cette requête d'action
            /// </summary>
            int NombreEnregistrementsAffectes { get; }

            /// <summary>
            /// Nouvel identifiant généré si la requête exécutée était de type INSERT INTO, sinon 0
            /// </summary>
            long NouvelIdentifiantGenere { get; }

            /// <summary>
            /// Message d'erreur signalée en cas d'échec
            /// </summary>
            string MessageErreur { get; }
        }

        /// <summary>
        /// Définit un retour de la méhode d'exécution d'une requête d'action
        /// </summary>
        private class ResultatExecution : IResultatExecution
        {
            #region Membres privés
            /// <summary>
            /// Indicateur de réussite ou d'échec
            /// </summary>
            private bool m_Reussite;

            /// <summary>
            /// Nombre d'enregistrements affectés par cette requête d'action
            /// </summary>
            private int m_NombreEnregistrementsAffectes;

            /// <summary>
            /// Nouvel identifiant généré si la requête exécutée était de type INSERT INTO, sinon 0
            /// </summary>
            private long m_NouvelIdentifiantGenere;

            /// <summary>
            /// Message d'erreur signalée en cas d'échec
            /// </summary>
            private string m_MessageErreur;
            #endregion

            #region Propriétés publiques en lecture seule
            /// <summary>
            /// Indicateur de réussite ou d'échec
            /// </summary>
            public bool Reussite
            {
                get
                {
                    return m_Reussite;
                }
            }

            /// <summary>
            /// Nombre d'enregistrements affectés par cette requête d'action
            /// </summary>
            public int NombreEnregistrementsAffectes
            {
                get
                {
                    return m_NombreEnregistrementsAffectes;
                }
            }

            /// <summary>
            /// Nouvel identifiant généré si la requête exécutée était de type INSERT INTO, sinon 0
            /// </summary>
            public long NouvelIdentifiantGenere
            {
                get
                {
                    return m_NouvelIdentifiantGenere;
                }
            }

            /// <summary>
            /// Message d'erreur signalée en cas d'échec
            /// </summary>
            public string MessageErreur
            {
                get
                {
                    return m_MessageErreur;
                }
            }
            #endregion

            #region Constructeurs
            /// <summary>
            /// Constructeur d'un résultat d'exécution de requête INSERT INTO qui a réussi
            /// </summary>
            /// <param name="NombreEnregistrementsAffectes">Nombre d'enregistremens affectés par cette requête d'action</param>
            /// <param name="NouvelIdentifiantGenere">Nouvel identifiant généré</param>
            public ResultatExecution(int NombreEnregistrementsAffectes, long NouvelIdentifiantGenere)
            {
                m_Reussite = true;
                m_NombreEnregistrementsAffectes = NombreEnregistrementsAffectes;
                m_NouvelIdentifiantGenere = NouvelIdentifiantGenere;
                m_MessageErreur = string.Empty;
            }

            /// <summary>
            /// Constructeur d'un résultat d'exécution de requête d'action (autre que INSERT INTO) qui a réussi
            /// </summary>
            /// <param name="NombreEnregistrementsAffectes">Nombre d'enregistremens affectés par cette requête d'action</param>
            public ResultatExecution(int NombreEnregistrementsAffectes)
            {
                m_Reussite = true;
                m_NombreEnregistrementsAffectes = NombreEnregistrementsAffectes;
                m_NouvelIdentifiantGenere = 0;
                m_MessageErreur = string.Empty;
            }

            /// <summary>
            /// Constructeur d'un résultat d'exécution de requête d'action qui a échoué
            /// </summary>
            /// <param name="MessageErreur">Message d'erreur</param>
            public ResultatExecution(string MessageErreur)
            {
                m_Reussite = false;
                m_NombreEnregistrementsAffectes = 0;
                m_NouvelIdentifiantGenere = 0;
                m_MessageErreur = MessageErreur;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// Permet d'exécuter une requête d'action
        /// </summary>
        /// <param name="Requete">Requête SQL à exécuter (doit être de type INSERT, UPDATE ou DELETE)</param>
        /// <param name="Valeurs">Valeurs des parties variables</param>
        /// <returns>Objet décrivant le résultat de l'opération</returns>
        public IResultatExecution Executer(string Requete, params object[] Valeurs)
        {
            string MessageErreur = string.Empty;
            for (int NombreEchecs = 0; NombreEchecs < 2; NombreEchecs++)
            {
                MyDB ConnexionUtilisee = null;
                try
                {
                    // Vérification de l'état théorique de la connexion
                    if (!EstConnecte) throw new Exception("Pas de connexion établie au serveur MySql");
                    // Vérification qu'il s'agit bien d'une requête d'action
                    if (EstRequeteConsultation(Requete)) throw new Exception("Une requête de consultation ne peut pas faire l'objet de la méthode MyDB.Executer()");
                    // Création d'une nouvelle connexion si il y a une lecture en cours
                    if (EstEnCoursDeLecture)
                    {
                        ConnexionUtilisee = new MyDB(this);
                        if (!ConnexionUtilisee.EstConnecte) throw new Exception("Aucune nouvelle connexion n'a pu être établie vers le serveur MySql");
                    }
                    else
                    {
                        ConnexionUtilisee = this;
                    }
                    // Création de la commande SQL
                    MySqlCommand Commande = ConnexionUtilisee.CreerCommande(Requete, Valeurs);
                    int NombreEnregistrementsAffectes = Commande.ExecuteNonQuery();
                    if (EstRequeteInsertion(Requete))
                    {
                        if (ConnexionUtilisee != this) ConnexionUtilisee.Dispose();
                        return new ResultatExecution(NombreEnregistrementsAffectes, Commande.LastInsertedId);
                    }
                    else
                    {
                        if (ConnexionUtilisee != this) ConnexionUtilisee.Dispose();
                        return new ResultatExecution(NombreEnregistrementsAffectes);
                    }
                }
                catch (Exception Erreur)
                {
                    /*
                    System.Diagnostics.Debug.WriteLine(string.Format(
                        "\nMyDB.Executer({0}, {1}) a échoué :\n{2}\n",
                        Requete,
                        string.Join("", Valeurs.Select(Valeur => string.Format(", {0}", Valeur))),
                        Erreur.Message));
                    */
                    if ((ConnexionUtilisee != null) && (ConnexionUtilisee != this)) ConnexionUtilisee.Dispose();
                    // Sauvegarde du message d'erreur au cas où il faudrait le retourner
                    MessageErreur = Erreur.Message;
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
            return new ResultatExecution(MessageErreur);
        }
    }
}
