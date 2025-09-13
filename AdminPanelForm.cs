using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flourish___Blotts
{
    public partial class AdminPanelForm : Form
    {
        public AdminPanelForm()
        {
            InitializeComponent();
        }


        private void LoadPage(UserControl page)
        {
            pnlMenu.Controls.Clear();   // clear old content
            page.Dock = DockStyle.Fill;        // fill the panel
            pnlMenu.Controls.Add(page); // add new content
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            LoadPage(new BookPage());
        }

        private void AdminPanelForm_Load(object sender, EventArgs e)
        {
            LoadPage(new HomePage());
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LoadPage(new HomePage());
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            LoadPage(new SalesPage());
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            LoadPage(new EmployeePage());
        }







        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void btnRequests_Click(object sender, EventArgs e)
        {
            LoadPage(new RequestsPage());
        }
    }
}
