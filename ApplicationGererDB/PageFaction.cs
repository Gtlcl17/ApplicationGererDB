using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDSGBD;

namespace ApplicationGererDB
{
    public partial class PageFaction : UserControl
    {
        private static readonly MyDB.CodeSql c_ClauseOrderBy_Nom = new MyDB.CodeSql("ORDER BY fa_name ASC");

        public PageFaction()
        {
            InitializeComponent();
            listeDeroulanteFaction1.Factions = Program.GMBD.EnumererFactions(null, c_ClauseOrderBy_Nom);
            // Selected index changed
            //listeDeroulanteFaction1.SurChangementSelection += new System.EventHandler(this.listeDeroulanteFaction_SurChangementSelection);
            //listeDeroulanteFaction1.SurChangementSelection += new System.EventHandler(this.lis)
            // 
        }
/*
        private void listeDeroulanteFaction_SurChangementSelection(object sender, EventArgs e)
        {
            if (listeDeroulanteFaction1.FactionSelectionnee != null)
            {
                Liste listeDeroulanteFaction1.FactionSelectionnee.SousFactions;
                ;
            }
            else
            {
                listAppareilsZones.AppareilPresents = null;
            }
        }

        private void listeDeroulanteSousFaction_SurChangementSelection(object sender, EventArgs e)
        {
            ListeSubFaction = listTypesAppareils.AppareilSelectionne;
            buttonModifierAppareil.Enabled = (listTypesAppareils.AppareilSelectionne != null);
        }
        */
    }
}
