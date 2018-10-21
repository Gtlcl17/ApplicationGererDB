using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSGBD;

namespace ApplicationGererDB
{
	/// <summary>
	/// Classe de type Model Factory permettant de manipuler les modèles à partir et vers la base de données MYSQL
	/// </summary>
	class GMBD
	{
		private static readonly MyDB.CodeSql c_NomTable_Utilisateur = new MyDB.CodeSql(new Utilisateur().NomDeLaTablePrincipale);
        private static readonly MyDB.CodeSql c_NomTable_Faction = new MyDB.CodeSql(new Faction().NomDeLaTablePrincipale);
        // <summary>
        /// Référence l'objet de connexion au serveur de base de données MySql
        /// </summary>
        private MyDB m_BD;

		/// <summary>
		/// Référence de l'objet de connexion au serveur MySQL
		/// </summary>
		public MyDB BD
		{
			get
			{
				return m_BD;
			}
		}

		/// <summary>
		/// Constructeur par défaut
		/// </summary>
		public GMBD()
		{
			m_BD = new MyDB("pmaroot", "y1vAb2NtVuX2XXQv", "warhammergt");
			m_BD.SurErreur += (ConnexionEmettrice, MethodeEmettrice, RequeteSql, Valeurs, MessageErreur) =>
			{
				System.Diagnostics.Debug.WriteLine(string.Format("\nERREUR SQL :\nMéthode : {0}\nRequête initiale : {1}\nValeurs des {2} parties variables : {3}\nRequête exécutée : {4}\nMessage d'erreur : {5}\n",
					MethodeEmettrice,
					RequeteSql,
					(Valeurs != null) ? Valeurs.Length : 0,
					((Valeurs != null) && (Valeurs.Length >= 1)) ? "\n* " + string.Join("\n* ", Valeurs.Select((Valeur, Indice) => string.Format("Valeurs[{0}] : {1}", Indice, (Valeur != null) ? Valeur.ToString() : "NULL")).ToArray()) : string.Empty,
					MyDB.FormaterEnSql(RequeteSql, Valeurs),
					MessageErreur));
			};
		}
		/// <summary>
		/// Permet d'initialiser ce gestionnaire de modèles
		/// </summary>
		/// <returns></returns>
		public bool Initialiser()
		{
			return m_BD.SeConnecter();
		}

		public Utilisateur ObtenirUtilisateur(int Id)
		{
			return EnumererUtilisateurs(new MyDB.CodeSql("WHERE id = {0}", Id), null).FirstOrDefault();
		}

		public IEnumerable<Utilisateur> EnumererUtilisateurs(MyDB.CodeSql ClauseWhere, MyDB.CodeSql ClauseOrderBy)
		{
			if (ClauseWhere == null) ClauseWhere = MyDB.CodeSql.Vide;
			if (ClauseOrderBy == null) ClauseOrderBy = MyDB.CodeSql.Vide;
			return Utilisateur.Enumerer(m_BD, m_BD.Enumerer("SELECT * FROM {0} {1} {2}", c_NomTable_Utilisateur, ClauseWhere, ClauseOrderBy));

		}

        public IEnumerable<Faction> EnumererFactions(MyDB.CodeSql ClauseWhere, MyDB.CodeSql ClauseOrderBy)
        {
            if (ClauseWhere == null) ClauseWhere = MyDB.CodeSql.Vide;
            if (ClauseOrderBy == null) ClauseOrderBy = MyDB.CodeSql.Vide;
            return Faction.Enumerer(m_BD, m_BD.Enumerer("SELECT * FROM {0} {1} {2}", c_NomTable_Faction, ClauseWhere, ClauseOrderBy));

        }
    }
}
