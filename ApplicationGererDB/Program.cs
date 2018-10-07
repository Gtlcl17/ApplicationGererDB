using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDSGBD;

namespace ApplicationGererDB
{
	static class Program
	{
		/// <summary>
		/// Référence l'objet de connexion au serveur de base de données MySql
		/// </summary>
		private static GMBD s_GMBD;

		/// <summary>
		/// Données du programme
		/// </summary>
		public static GMBD GMBD
		{
			get
			{
				return s_GMBD;
			}
		}

		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			s_GMBD = new GMBD();
			s_GMBD.BD.SurChangementEtatConnexion += BD_SurChangementEtatConnexion;

			if(!s_GMBD.Initialiser())
			{
				MessageBox.Show("Erreur d'accès à la base de données!", "GestUtil", 
									MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
				return;
			}

			#region tests
			// Boucle affichant chaque utilisateur présent dans la db avec son nom, prénom et son ou ses rôles

			#endregion



			Application.Run(new Form1());

			
		}

		/// <summary>
		/// Méthode appelée lorsqu'un changement d'état de la connexion au serveur MySQL se produit
		/// </summary>
		/// <param name="ConnexionEmetrice">Connexion concernée par le changement d'état</param>
		/// <param name="EtatPrecedent">Etat précédant ce changement</param>
		/// <param name="NouvelEtat">Nouvel état résultant de ce changement</param>
		private static void BD_SurChangementEtatConnexion(MyDB ConnexionEmetrice, MyDB.EtatConnexion EtatPrecedent, MyDB.EtatConnexion NouvelEtat)
		{
			System.Diagnostics.Debug.WriteLine(string.Format("\nCONNEXION BD :\nChangement d'état : {0} ==>> {1}\n", EtatPrecedent, NouvelEtat));
		}
	}
}
