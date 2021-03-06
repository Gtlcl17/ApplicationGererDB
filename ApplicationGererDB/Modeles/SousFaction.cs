﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGererDB
{
	public class SousFaction : Entite<SousFaction, SousFaction.Champ>
	{
		/// <summary>
		/// Champ décrivant cette SousFaction
		/// </summary>
		public enum Champ
		{
			/// <summary>
			/// Identifiant de cette SousFaction
			/// </summary>
			Id,
			/// <summary>
			/// name de ce SousFaction
			/// </summary>
			Name
		}

        #region Membres privés
        /// <summary>
        /// Stocke la valeur du champ Name
        /// </summary>
        private string m_Name;

        #endregion

        #region Membres publics
        /// <summary>
        /// Name de ce SousFaction
        /// </summary>
        public string Name
        {
			get
			{
				return m_Name;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					Declencher_SurErreur(this, Champ.Name, "Nom vide ou ne contient que des espaces");
				}
				else
				{
					value = value.Trim();
					if (!string.Equals(value, m_Name))
					{
						ModifierChamp(Champ.Name, ref m_Name, value);
					}
				}
			}
		}

		
		#endregion

		#region Constructeur
		/// <summary>
		/// Constructeur par défaut
		/// </summary>
		public SousFaction()
            : base()
        {
			m_Name = string.Empty;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Id">Identifiant de ce SousFaction</param>
		/// <param name="Name">Nom de ce SousFaction</param>
		public SousFaction(int Id, string Name)
            : this()
        {
			DefinirId(Id);
			this.Name = Name;
		}

		/// <summary>
		/// Constructeur spécifique
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrement">Enregistrement d'où extraire les valeurs de champs</param>
		public SousFaction(PDSGBD.MyDB Connexion, PDSGBD.MyDB.IEnregistrement Enregistrement)
            : this()
        {
			base.Connexion = Connexion;
			if (Enregistrement != null)
			{
				DefinirId(Enregistrement.ValeurChampComplet<int>(NomDeLaTablePrincipale, "fa_id"));
				this.Name = Enregistrement.ValeurChampComplet<string>(NomDeLaTablePrincipale, "fa_name");
			}
		}

		#endregion

		/// <summary>
		/// Permet d'énumérer les entités correspondant aux enregistrements énumérés
		/// </summary>
		/// <param name="Connexion">Connexion au serveur MySQL</param>
		/// <param name="Enregistrements">Enregistrements énumérés, sources des entités à créer</param>
		/// <returns>Enumération des entités issues des enregistrements énumérés</returns>
		public static IEnumerable<SousFaction> Enumerer(PDSGBD.MyDB Connexion, IEnumerable<PDSGBD.MyDB.IEnregistrement> Enregistrements)
		{
			return Enumerer(Enregistrements, Enregistrement => new SousFaction(Connexion, Enregistrement));
		}

		
		#region Méthodes relatives au gestionnaire d'entités pour base de données MySQL
		/// <summary>
		/// Méthode retournant le name de la table principale de ce type d'entités
		/// </summary>
		/// <returns>name de la table principale de ce type d'entités</returns>
		public override string NomDeLaTablePrincipale
		{
			get
			{
				return "sousfaction";
			}
		}

		/// <summary>
		/// Clause d'assignation utilisable dans une requête INSERT/UPDATE
		/// </summary>
		public override PDSGBD.MyDB.CodeSql ClauseAssignation
		{
			get
			{
				return new PDSGBD.MyDB.CodeSql("sf_name = {0}", m_Name);
			}
		}
		
		#endregion

	}
}
