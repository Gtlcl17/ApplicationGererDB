using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationGererDB
{
    public partial class ListeDeroulanteFaction : UserControl
    {
        /// <summary>
        /// Classe à usage interne servant à encapsuler dans chaque Item de la ComboBox une entité de type Faction, et en proposant un ToString approprié à cette vue
        /// </summary>
        private class Element
        {
            /// <summary>
            /// Faction
            /// </summary>
            public Faction Faction { get; private set; }

            /// <summary>
            /// Constructeur spécifique
            /// </summary>
            /// <param name="Faction">Zône</param>
            public Element(Faction Faction)
            {
                this.Faction = Faction;
            }

            /// <summary>
            /// Réécriture de la méthode de test d'égalité
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return (obj is Element) ? Faction.Id.Equals((obj as Element).Faction.Id) : false;
            }

            /// <summary>
            /// Réécriture de la méthode fournissant une clé de hachage
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Faction.Id.GetHashCode();
            }

            /// <summary>
            /// Méthode retournant le texte qui sera représentatif dans la ComboBox de la zône considérée
            /// </summary>
            /// <returns>Chaîne représentant cet élément</returns>
            public override string ToString()
            {
                return Faction.Name;
            }
        }

        /// <summary>
        /// Enumération des factions générées par cette vue
        /// </summary>
        public IEnumerable<Faction> Factions
        {
            get
            {
                return comboFaction.Items.OfType<Element>().Select(Element => Element.Faction);
            }
            set
            {
                if(value != null)
                {
                    comboFaction.Items.Clear();
                    foreach (Faction Faction in value)
                    {
                        comboFaction.Items.Add(new Element(Faction));
                    }
                }
            }
        }

        public ListeDeroulanteFaction()
        {
            InitializeComponent();

        }
    }
}
