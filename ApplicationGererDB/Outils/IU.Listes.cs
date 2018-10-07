using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ApplicationGererDB
{
	/// <summary>
	/// Contient des outils relatifs à la gestion des listes en Windows Forms
	/// </summary>
	public static partial class IU
    {
        public static void Remplir<T>(ComboBox Liste, IEnumerable<T> Elements)
        {
            T ElementSelectionne = (Liste.SelectedItem is T) ? (T)Liste.SelectedItem : default(T);
            int IndiceASelectionner = -1;
            Liste.Items.Clear();
            int Indice = 0;
            if (Elements != null)
            {
                foreach (T Element in Elements)
                {
                    Liste.Items.Add(Element);
                    if ((IndiceASelectionner < 0) && (Element != null) && Element.Equals(ElementSelectionne))
                    {
                        IndiceASelectionner = Indice;
                    }
                    Indice++;
                }
                if (IndiceASelectionner >= 0) Liste.SelectedIndex = IndiceASelectionner;
            }
        }

        public static void Initialiser(ListView Liste, params string[] Entetes)
        {
            Liste.FullRowSelect = true;
            Liste.MultiSelect = false;
            Liste.LabelEdit = false;
            Liste.GridLines = true;
            Liste.HideSelection = false;
            Liste.View = View.Details;
            if ((Entetes != null) && (Entetes.Length >= 1))
            {
                Liste.Columns.Clear();
                foreach (string Entete in Entetes)
                {
                    Liste.Columns.Add(Entete);
                }
            }
        }

        public static void Remplir<T>(ListView Liste, IEnumerable<T> Elements, Func<T, string[]> ExtracteurChamps)
        {
            Liste.Items.Clear();
            if (Elements != null)
            {
                foreach (T Element in Elements)
                {
                    Liste.Items.Add(new ListViewItem(ExtracteurChamps(Element))).Tag = Element;
                }
                foreach (ColumnHeader Colonne in Liste.Columns)
                {
                    Colonne.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }
    }
}
