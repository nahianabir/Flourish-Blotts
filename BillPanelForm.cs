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
    public partial class BillPanelForm : Form
    {
        private DataAccess Da { get; set; }
        public string SalesmanID { get; set; }
        public BillPanelForm()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateCartGridView();
        }

        private void PopulateCartGridView(string sql = "select * from Cart;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvCart.AutoGenerateColumns = false;
            this.dgvCart.DataSource = ds.Tables[0];
        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtCustomerName.Text) ||
                    string.IsNullOrWhiteSpace(this.txtCustomerPhone.Text) ||
                    string.IsNullOrWhiteSpace(this.cmbPaymentType.Text) ||
                    string.IsNullOrWhiteSpace(this.txtSalesmanID.Text))
                {
                    MessageBox.Show("Please enter all details and make sure salesman is logged in.");
                    return;
                }

                // Get cart items
                var dtCart = this.Da.ExecuteQueryTable("select * from Cart;");
                if (dtCart.Rows.Count == 0)
                {
                    MessageBox.Show("Cart is empty.");
                    return;
                }

                // Insert each cart item into Sales table
                foreach (DataRow row in dtCart.Rows)
                {
                    var sqlSale = "insert into Sales (CustomerName, CustomerPhone, PaymentType, ISBN, BookName, Quantity, Price, TotalPrice, SalesmanID) " +
                                  "values ('" + this.txtCustomerName.Text + "', '" + this.txtCustomerPhone.Text + "', '" + this.cmbPaymentType.Text + "', " +
                                  "'" + row["ISBN"] + "', '" + row["Name"] + "', " + row["Quantity"] + ", " + row["Price"] + ", " + row["TotalPrice"] + ", '" + this.txtSalesmanID + "');";

                    this.Da.ExecuteDMLQuery(sqlSale);
                }

                // Clear cart
                this.Da.ExecuteDMLQuery("delete from Cart;");

                MessageBox.Show("Sale completed successfully!");

                // Refresh Book stock + Cart grid
                //this.PopulateGridView();
                this.PopulateCartGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BillPanelForm_Load(object sender, EventArgs e)
        {
            txtSalesmanID.Text = SalesmanID;
        }
    }
}
