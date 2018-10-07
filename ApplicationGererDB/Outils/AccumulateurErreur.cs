using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	/// <summary>
	/// Accumulateur de notification d'erreur
	/// <para>Utilisé entre autre comme paramètre des méthodes AvantChangement</para>
	/// </summary>
	public class AccumulateurErreur
    {
        #region Membres privés
        private bool m_Accepte;
        private string m_MessageErreur;
        #endregion

        /// <summary>
        /// Indique qu'il n'y a aucune erreur notifiée
        /// </summary>
        public bool Accepte
        {
            get
            {
                return m_Accepte;
            }
        }

        /// <summary>
        /// Indique qu'il y a au moins une erreur notifiée
        /// </summary>
        public bool Refus
        {
            get
            {
                return !m_Accepte;
            }
        }

        /// <summary>
        /// Message d'erreur(s)
        /// </summary>
        public string MessageErreur
        {
            get
            {
                return m_MessageErreur;
            }
        }

        /// <summary>
        /// Notifier une erreur (soit la première, soit une complémentaire)
        /// </summary>
        /// <param name="MessageErreur">Message décrivant l'erreur notifiée</param>
        public void NotifierErreur(string MessageErreur)
        {
            m_Accepte = false;
            if (!string.IsNullOrEmpty(m_MessageErreur)) m_MessageErreur += "\n";
            m_MessageErreur += MessageErreur;
        }

        /// <summary>
        /// Initialise cet accumulateur d'erreur en considérant par défaut qu'il n'y a aucune erreur
        /// </summary>
        public AccumulateurErreur()
        {
            m_Accepte = true;
            m_MessageErreur = string.Empty;
        }
    }
}
