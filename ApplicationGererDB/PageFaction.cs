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
            
        }
    }
}
