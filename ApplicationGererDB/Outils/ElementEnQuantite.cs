using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	/// <summary>
	/// Classe générique de base permettant d'associer une quantité à un objet
	/// </summary>
	/// <typeparam name="T">Type d'objet</typeparam>
	public abstract class ElementEnQuantite<T>
        where T : class
    {
        /// <summary>
        /// Stocke la référence de l'objet
        /// </summary>
        private T m_Element;

        /// <summary>
        /// Stocke la quantité (positive ou nulle) de cet objet
        /// </summary>
        private int m_Quantite;

        /// <summary>
        /// Objet concerné par cet élément
        /// </summary>
        public T Element
        {
            get
            {
                return m_Element;
            }
            protected set
            {
                if ((value != null) && (m_Element == null))
                {
                    m_Element = value;
                }
            }
        }

        /// <summary>
        /// Quantité (valeur entière supérieure ou égale à zéro) de cet objet
        /// </summary>
        public int Quantite
        {
            get
            {
                return m_Quantite;
            }
            protected set
            {
                if ((value >= 0) && (value != m_Quantite))
                {
                    m_Quantite = value;
                }
            }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        protected ElementEnQuantite()
        {
            m_Element = null;
            m_Quantite = 0;
        }

        /// <summary>
        /// Constructeur spécifique
        /// </summary>
        /// <param name="Element">Objet concerné par cet élément</param>
        /// <param name="QuantiteInitiale">Quantité initiale de cet objet</param>
        protected ElementEnQuantite(T Element, int QuantiteInitiale = 0)
            : this()
        {
            m_Element = Element;
            this.Quantite = QuantiteInitiale;
        }
    }
}
