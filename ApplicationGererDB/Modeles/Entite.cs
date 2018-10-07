using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
    /// <summary>
    /// Fournit les fonctionnalités relatives à la gestion d'entités avec une base de données MySQL
    /// </summary>
    public interface IEntiteMySQL
    {
        /// <summary>
        /// Nom de la table principale de ce type d'entités
        /// </summary>
        string NomDeLaTablePrincipale { get; }

        /// <summary>
        /// Permet de (re)définir l'identifiant de cette entité suite à l'exécution réussie d'une requête INSERT INTO
        /// </summary>
        /// <param name="ResultatExecution">Résultat de l'exécution d'une requête INSERT INTO appropriée</param>
        /// <returns>Vrai si l'identifiant a été (re)défini, sinon faux</returns>
        bool DefinirId(PDSGBD.MyDB.IResultatExecution ResultatExecution);

        /// <summary>
        /// Clause d'assignation utilisable dans une requête INSERT/UPDATE
        /// </summary>
        PDSGBD.MyDB.CodeSql ClauseAssignation { get; }

        /// <summary>
        /// Permet de supprimer tous les enregistrements liés à cette entité
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        void SupprimerEnCascade(PDSGBD.MyDB Connexion);
    }

    /// <summary>
    /// Classe de base décrivant une entité
    /// </summary>
    public abstract class Entite<TEntite, TChamp> : IEntiteMySQL
        where TEntite : Entite<TEntite, TChamp>, new()
        where TChamp : struct, IConvertible
    {
        /// <summary>
        /// Informations culturelles anglaises
        /// </summary>
        private static readonly System.Globalization.CultureInfo c_CultureAnglaise = System.Globalization.CultureInfo.GetCultureInfo("EN-US");

        /// <summary>
        /// Méthode pouvant être attachée à l'événement AvantChangement
        /// </summary>
        /// <param name="Entite">Entité concernée par le changement</param>
        /// <param name="Champ">Champ concerné par le changement</param>
        /// <param name="ValeurActuelle">Valeur actuelle du champ concerné par le changement</param>
        /// <param name="NouvelleValeur">Nouvelle valeur à affecter au champ concerné par le changement</param>
        /// <param name="AccumulateurErreur">Accumulateur de notification d'erreur servant à refuser le changement</param>
        public delegate void MethodeAvantChangement(TEntite Entite, TChamp Champ, object ValeurActuelle, object NouvelleValeur, AccumulateurErreur AccumulateurErreur);

        /// <summary>
        /// Méthode pouvant être attachée à l'événement ApresChangement
        /// </summary>
        /// <param name="Entite">Entité concernée par le changement</param>
        /// <param name="Champ">Champ concerné par le changement</param>
        /// <param name="ValeurPrecedente">Valeur du champ concerné avant son changement</param>
        /// <param name="ValeurActuelle">Valeur actuelle du champ après son changement</param>
        public delegate void MethodeApresChangement(TEntite Entite, TChamp Champ, object ValeurPrecedente, object ValeurActuelle);

        /// <summary>
        /// Méthode pouvant être attachée à l'événement SurErreur
        /// </summary>
        /// <param name="Entite">Entité concernée par l'erreur</param>
        /// <param name="Champ">Champ concerné par l'erreur</param>
        /// <param name="MessageErreur">Message de l'erreur</param>
        public delegate void MethodeSurErreur(TEntite Entite, TChamp Champ, string MessageErreur);

        #region Membres privés
        /// <summary>
        /// Indicateurs de validité des champs
        /// </summary>
        private bool[] m_ChampsValides;

        /// <summary>
        /// Identifiant de l'enregistrement "maître"
        /// </summary>
        private int m_Id;

        /// <summary>
        /// Référence de la connexion au gestionnaire de bases de données MySQL
        /// </summary>
        private PDSGBD.MyDB m_Connexion;
        #endregion

        /// <summary>
        /// Evénement appelé avant que la valeur d'un champ ne soit modifiée
        /// <para>Possibilité d'annulation de la modification par notification d'une erreur sur l'accumulateur d'erreur</para>
        /// </summary>
        public event MethodeAvantChangement AvantChangement = null;

        /// <summary>
        /// Evénement appelé après que la valeur d'un champ ait été modifiée
        /// </summary>
        public event MethodeApresChangement ApresChangement = null;

        /// <summary>
        /// Evénement appelé en cas d'erreur lors d'une tentative de modification de la valeur d'un champ (valeur intrinsèquement non valide, ou refusée avant modification)
        /// </summary>
        public event MethodeSurErreur SurErreur = null;

        /// <summary>
        /// Identifiant de cette entité
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// Indique si les champs de cette entité sont tous valides
        /// </summary>
        public bool EstValide
        {
            get
            {
                return !m_ChampsValides.Contains(false);
            }
        }

        /// <summary>
        /// Indique si le champ spécifié est valide ou pas
        /// </summary>
        /// <param name="Champ">Champ pour lequel on désire connaître son état de validité</param>
        /// <returns>Vrai si le champ spécifié est valide, sinon faux</returns>
        public bool ChampEstValide(TChamp Champ)
        {
            return m_ChampsValides[Champ.ToInt32(c_CultureAnglaise)];
        }

        /// <summary>
        /// Indique si le champ spécifié est valide ou pas
        /// </summary>
        /// <param name="Champ">Champ pour lequel on désire connaître son état de validité</param>
        /// <returns>Vrai si le champ spécifié est valide, sinon faux</returns>
        protected void ChampEstValide(TChamp Champ, bool Valeur)
        {
            m_ChampsValides[Champ.ToInt32(c_CultureAnglaise)] = Valeur;
        }

        /// <summary>
        /// Méthode pouvant être attachée à l'événement AvantChangement
        /// </summary>
        /// <param name="Entite">Entité concernée par le changement</param>
        /// <param name="Champ">Champ concerné par le changement</param>
        /// <param name="ValeurActuelle">Valeur actuelle du champ concerné par le changement</param>
        /// <param name="NouvelleValeur">Nouvelle valeur à affecter au champ concerné par le changement</param>
        /// <param name="AccumulateurErreur">Accumulateur de notification d'erreur servant à refuser le changement</param>
        protected void Declencher_AvantChangement(TEntite Entite, TChamp Champ, object ValeurActuelle, object NouvelleValeur, AccumulateurErreur AccumulateurErreur)
        {
            if (AvantChangement != null)
            {
                AvantChangement(Entite, Champ, ValeurActuelle, NouvelleValeur, AccumulateurErreur);
            }
        }

        /// <summary>
        /// Méthode pouvant être attachée à l'événement ApresChangement
        /// </summary>
        /// <param name="Entite">Entité concernée par le changement</param>
        /// <param name="Champ">Champ concerné par le changement</param>
        /// <param name="ValeurPrecedente">Valeur du champ concerné avant son changement</param>
        /// <param name="ValeurActuelle">Valeur actuelle du champ après son changement</param>
        protected void Declencher_ApresChangement(TEntite Entite, TChamp Champ, object ValeurPrecedente, object ValeurActuelle)
        {
            if (ApresChangement != null)
            {
                ApresChangement(Entite, Champ, ValeurPrecedente, ValeurActuelle);
            }
        }

        /// <summary>
        /// Méthode pouvant être attachée à l'événement SurErreur
        /// </summary>
        /// <param name="Entite">Entité concernée par l'erreur</param>
        /// <param name="Champ">Champ concerné par l'erreur</param>
        /// <param name="MessageErreur">Message de l'erreur</param>
        protected void Declencher_SurErreur(TEntite Entite, TChamp Champ, string MessageErreur)
        {
            if (SurErreur != null)
            {
                SurErreur(Entite, Champ, MessageErreur);
            }
        }

        /// <summary>
        /// Connexion au gestionnaire de bases de données MySQL
        /// </summary>
        protected PDSGBD.MyDB Connexion
        {
            get
            {
                return m_Connexion;
            }
            set
            {
                if ((m_Connexion == null) && (value != null))
                {
                    m_Connexion = value;
                }
            }
        }

        /// <summary>
        /// Indique si au moins une méthode est attachée à l'événement AvantChangement
        /// </summary>
        private bool MethodeAttacheeA_AvantChangement
        {
            get
            {
                return (AvantChangement != null);
            }
        }

        /// <summary>
        /// Indique si au moins une méthode est attachée à l'événement ApresChangement
        /// </summary>
        private bool MethodeAttacheeA_ApresChangement
        {
            get
            {
                return (ApresChangement != null);
            }
        }

        /// <summary>
        /// Indique si au moins une méthode est attachée à l'événement SurErreur
        /// </summary>
        private bool MethodeAttacheeA_SurErreur
        {
            get
            {
                return (SurErreur != null);
            }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        protected Entite()
        {
            m_ChampsValides = new bool[System.Enum.GetValues(typeof(TChamp)).Length];
            m_ChampsValides[0] = true;
            m_Id = 0;
            m_Connexion = null;
        }

        /// <summary>
        /// Permet d'énumérer les entités correspondant aux enregistrements énumérés
        /// </summary>
        /// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
        /// <param name="CreateurEntite">Méthode permettant de créer une nouvelle entité à partir de l'enregistrement spécifié</param>
        /// <returns>Enumération des entités issues des enregistrements énumérés</returns>
        protected static IEnumerable<TEntite> Enumerer(IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements, Func<PDSGBD.MyDB.IEnregistrement, TEntite> CreateurEntite)
        {
            if (CreateurEntite != null)
            {
                foreach (PDSGBD.MyDB.IEnregistrement Enregistrement in Enregistrements)
                {
                    yield return CreateurEntite(Enregistrement);
                }
            }
        }

        /// <summary>
        /// Méthode permettant de réaliser la modification d'un membre privé par le biais du set d'une propriété publique correspondant à un champ particulier de l'entité
        /// </summary>
        /// <typeparam name="T">Type de données du membre privé (et donc de cette propriété publique)</typeparam>
        /// <param name="ChampConcerne">Valeur enumérée représentant le champ géré par ce membre privé et cette propriété publique</param>
        /// <param name="MembrePrive">Membre privé à modifier</param>
        /// <param name="NouvelleValeur">Nouvelle valeur à attribuer à ce membre privé</param>
        /// <returns>Vrai si la modification a été acceptée et réalisée, sinon faux</returns>
        protected bool ModifierChamp<T>(TChamp ChampConcerne, ref T MembrePrive, T NouvelleValeur)
        {
            return ModifierChamp<T, T>(ChampConcerne, ref MembrePrive, NouvelleValeur, null);
        }

        /// <summary>
        /// Méthode permettant de réaliser la modification d'un membre privé par le biais du set d'une propriété publique correspondant à un champ particulier de l'entité
        /// </summary>
        /// <typeparam name="T">Type de données du membre privé (et donc de cette propriété publique)</typeparam>
        /// <typeparam name="U">Type de données de la propriété publique exposant la valeur de ce membre privé</typeparam>
        /// <param name="ChampConcerne">Valeur enumérée représentant le champ géré par ce membre privé et cette propriété publique</param>
        /// <param name="MembrePrive">Membre privé à modifier</param>
        /// <param name="NouvelleValeur">Nouvelle valeur à attribuer à ce membre privé</param>
        /// <param name="TransformationMembreVersPropriete">Méthode optionelle permettant de réaliser la transformation d'une valeur type "membre privé" en une valeur type "propriété publique"</param>
        /// <returns>Vrai si la modification a été acceptée et réalisée, sinon faux</returns>
        protected bool ModifierChamp<T, U>(TChamp ChampConcerne, ref T MembrePrive, T NouvelleValeur, Func<T, U> TransformationMembreVersPropriete = null)
        {
            bool ModificationAcceptee = true;
            if (MethodeAttacheeA_AvantChangement)
            {
                AccumulateurErreur AccumulateurErreur = new AccumulateurErreur();
                if (TransformationMembreVersPropriete != null)
                {
                    Declencher_AvantChangement(this as TEntite, ChampConcerne, TransformationMembreVersPropriete(MembrePrive), TransformationMembreVersPropriete(NouvelleValeur), AccumulateurErreur);
                }
                else
                {
                    Declencher_AvantChangement(this as TEntite, ChampConcerne, MembrePrive, NouvelleValeur, AccumulateurErreur);
                }
                ModificationAcceptee = AccumulateurErreur.Accepte;
                if (!ModificationAcceptee)
                {
                    Declencher_SurErreur(this as TEntite, ChampConcerne, AccumulateurErreur.MessageErreur);
                }
            }
            if (ModificationAcceptee)
            {
                if (MethodeAttacheeA_ApresChangement)
                {
                    T ValeurPrecedente = MembrePrive;
                    MembrePrive = NouvelleValeur;
                    if (TransformationMembreVersPropriete != null)
                    {
                        Declencher_ApresChangement(this as TEntite, ChampConcerne, TransformationMembreVersPropriete(ValeurPrecedente), TransformationMembreVersPropriete(MembrePrive));
                    }
                    else
                    {
                        Declencher_ApresChangement(this as TEntite, ChampConcerne, ValeurPrecedente, MembrePrive);
                    }
                }
                else
                {
                    MembrePrive = NouvelleValeur;
                }
                ChampEstValide(ChampConcerne, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Permet de définir l'identifiant de cette entité
        /// </summary>
        /// <param name="Id">Identifiant</param>
        /// <returns>Vrai si l'identifiant a été défini, sinon faux</returns>
        protected bool DefinirId(int Id)
        {
            if ((m_Id != 0) || (Id <= 0)) return false;
            m_Id = Id;
            return true;
        }

        public virtual bool Enregistrer(PDSGBD.MyDB Connexion, TEntite Entite, bool RecreationAutorisee = false)
        {
            if (!Entite.EstValide) return false;
            PDSGBD.MyDB.CodeSql NomTable = new PDSGBD.MyDB.CodeSql(NomDeLaTablePrincipale);
            if (Entite.Id > 0)
            {
                if (Connexion.Executer("UPDATE {0} SET {1} WHERE id = {2}", NomTable, Entite.ClauseAssignation, Entite.Id).Reussite) return true;
                if (Connexion.ValeurDe<long>("SELECT COUNT(*) FROM {0} WHERE id = {1}", NomTable, Entite.Id) == 1) return false;
                if (!RecreationAutorisee) return false;
            }
            return Entite.DefinirId(Connexion.Executer("INSERT INTO {0} SET {1}", NomTable, Entite.ClauseAssignation));
        }

        public virtual bool Supprimer(PDSGBD.MyDB Connexion, TEntite Entite, bool SuppressionEnCascade = false)
        {
            if (SuppressionEnCascade) Entite.SupprimerEnCascade(Connexion);
            PDSGBD.MyDB.CodeSql NomTable = new PDSGBD.MyDB.CodeSql(NomDeLaTablePrincipale);
            return Connexion.Executer("DELETE FROM {0} WHERE id = {1}", NomTable, Entite.Id).Reussite;
        }

        #region Fonctionnalités de l'interface IEntiteMySQL
        /// <summary>
        /// Nom de la table principale de ce type d'entités
        /// </summary>
        public abstract string NomDeLaTablePrincipale
        {
            get;
        }

        /// <summary>
        /// Permet de (re)définir l'identifiant de cette entité suite à l'exécution réussie d'une requête INSERT INTO
        /// </summary>
        /// <param name="ResultatExecution">Résultat de l'exécution d'une requête INSERT INTO appropriée</param>
        /// <returns>Vrai si l'identifiant a été (re)défini, sinon faux</returns>
        public bool DefinirId(PDSGBD.MyDB.IResultatExecution ResultatExecution)
        {
            if (!ResultatExecution.Reussite || (ResultatExecution.NouvelIdentifiantGenere == 0)) return false;
            m_Id = (int)ResultatExecution.NouvelIdentifiantGenere;
            return true;
        }

        /// <summary>
        /// Clause d'assignation utilisable dans une requête INSERT/UPDATE
        /// </summary>
        public abstract PDSGBD.MyDB.CodeSql ClauseAssignation
        {
            get;
        }

        /// <summary>
        /// Permet de supprimer tous les enregistrements liés à cette entité
        /// </summary>
        /// <param name="Connexion">Connexion au serveur MySQL</param>
        public virtual void SupprimerEnCascade(PDSGBD.MyDB Connexion)
        {
        }
        #endregion
    }
}
