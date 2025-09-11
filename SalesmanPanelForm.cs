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
    public partial class SalesmanPanelForm : Form
    {

        private DataAccess Da { get; set; }
        public SalesmanPanelForm()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();
        }

        //book grid view
        private void PopulateGridView(string sql = "select * from Book;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvBook.AutoGenerateColumns = false;
            this.dgvBook.DataSource = ds.Tables[0];
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();

            this.Close();
        }

        private void SalesmanPanelForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            this.txtISBN.Text = this.dgvBook.CurrentRow.Cells[0].Value.ToString();
            this.txtName.Text = this.dgvBook.CurrentRow.Cells[1].Value.ToString();
            this.txtPrice.Text = this.dgvBook.CurrentRow.Cells[6].Value.ToString();
        }
    }
}
