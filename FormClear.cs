using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flourish___Blotts
{
    internal class FormClear
    {
        public static void ClearAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();

                else if (c is ComboBox)
                    ((ComboBox)c).SelectedIndex = -1;

                else if (c is CheckBox)
                    ((CheckBox)c).Checked = false;

                else if (c is DataGridView)
                    ((DataGridView)c).ClearSelection();

                else if (c is DateTimePicker)
                    ((DateTimePicker)c).Value = DateTime.Now;

                else if (c.HasChildren)
                    ClearAllControls(c); // recursive for panels, groupboxes, etc.
            }
        }
    }
}
