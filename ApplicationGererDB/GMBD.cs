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
			m_BD = new MyDB("GestUtil_user", "Osj3MukXV1aaXYGc", "GestUtil");
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
	}
}
