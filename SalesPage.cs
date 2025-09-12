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
    public partial class SalesPage : UserControl
    {
        private DataAccess Da { get; set; }
        public SalesPage()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();
        }

        private void PopulateGridView(string sql = "select * from Sales;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvSales.AutoGenerateColumns = false;
            this.dgvSales.DataSource = ds.Tables[0];
        }

        private void SalesPage_Load(object sender, EventArgs e)
        {

        }
    }
}
