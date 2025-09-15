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
        public string LoggedInSalesmanID { get; set; }
        public SalesmanPanelForm()
        {
            InitializeComponent();
            this.dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Da = new DataAccess();
            this.PopulateGridView();
            this.PopulateCartGridView();

        }

        //book grid view
        private void PopulateGridView(string sql = "select * from Book;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvBook.AutoGenerateColumns = false;
            this.dgvBook.DataSource = ds.Tables[0];
        }


        //cart grid view
        private void PopulateCartGridView(string sql = "select * from Cart;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvCart.AutoGenerateColumns = false;
            this.dgvCart.DataSource = ds.Tables[0];
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SalesmanPanelForm_Load(object sender, EventArgs e)
        {
            this.dgvBook.ClearSelection();
            this.dgvCart.ClearSelection();
        }

        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            this.txtISBN.Text = this.dgvBook.CurrentRow.Cells[0].Value.ToString();
            this.txtName.Text = this.dgvBook.CurrentRow.Cells[1].Value.ToString();
            this.txtPrice.Text = this.dgvBook.CurrentRow.Cells[6].Value.ToString();
        }




        private void btnBill_Click(object sender, EventArgs e)
        {
            BillPanelForm bill = new BillPanelForm();
            bill.SalesmanID = this.LoggedInSalesmanID;
            DialogResult result = bill.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.PopulateCartGridView();
                this.CalculateCartTotal();
            }
        }

        private void txtAutoSearch_TextChanged(object sender, EventArgs e)
        {
            var sql = "select * from Book where Name like '" + this.txtAutoSearch.Text + "%';";
            this.PopulateGridView(sql);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtISBN.Text) ||
                    string.IsNullOrWhiteSpace(this.txtName.Text) ||
                    string.IsNullOrWhiteSpace(this.txtQuantity.Text) ||
                    string.IsNullOrWhiteSpace(this.txtPrice.Text))
                {
                    MessageBox.Show("Please fill all fields before adding to cart");
                    return;
                }

                int quantity = Convert.ToInt32(this.txtQuantity.Text);
                decimal price = Convert.ToDecimal(this.txtPrice.Text);

                // Check available stock
                var stockQuery = "select Quantity from Book where ISBN = '" + this.txtISBN.Text + "';";
                var dt = this.Da.ExecuteQueryTable(stockQuery);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Book not found in stock.");
                    return;
                }

                int availableStock = Convert.ToInt32(dt.Rows[0]["Quantity"]);
                if (quantity > availableStock)
                {
                    MessageBox.Show("Not enough stock available. Only " + availableStock + " left.");
                    return;
                }

                // Calculate total price
                decimal totalPrice = quantity * price;

                // Insert into Cart table
                var sqlInsert = "insert into Cart (ISBN, Name, Quantity, Price, TotalPrice) " +
                                "values ('" + this.txtISBN.Text + "', '" + this.txtName.Text + "', " + 
                                quantity + ", " + price + ", " + totalPrice + ");";

                var countInsert = this.Da.ExecuteDMLQuery(sqlInsert);

                if (countInsert == 1)
                {
                    // Decrease stock in Book table
                    var sqlUpdate = "update Book set Quantity = Quantity - " + quantity + " where ISBN = '" + this.txtISBN.Text + "';";
                    this.Da.ExecuteDMLQuery(sqlUpdate);

                    MessageBox.Show("Book added to cart and stock updated.");
                }
                else
                {
                    MessageBox.Show("Failed to add book to cart.");
                }

                this.PopulateGridView();
                this.PopulateCartGridView();
                this.CalculateCartTotal();

                FormClear.ClearAllControls(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void CalculateCartTotal()
        {
            try
            {
                var query = "select sum(TotalPrice) as GrandTotal from Cart;";
                var dt = this.Da.ExecuteQueryTable(query);

                if (dt.Rows.Count > 0 && dt.Rows[0]["GrandTotal"] != DBNull.Value)
                {
                    decimal grandTotal = Convert.ToDecimal(dt.Rows[0]["GrandTotal"]);
                    this.lblTotalPrice.Text = grandTotal.ToString("0.00");
                }
                else
                {
                    this.lblTotalPrice.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in calculating total: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCart.CurrentRow == null)
                {
                    MessageBox.Show("Please select a book to remove from the cart.");
                    return;
                }

                // Get selected row data
                int cartId = Convert.ToInt32(this.dgvCart.CurrentRow.Cells["Id"].Value);
                string isbn = this.dgvCart.CurrentRow.Cells["ISBN"].Value.ToString();
                int quantity = Convert.ToInt32(this.dgvCart.CurrentRow.Cells["Quantity"].Value);

                // Delete only that row
                var sqlDelete = "delete from Cart where Id = " + cartId + ";";
                int countDelete = this.Da.ExecuteDMLQuery(sqlDelete);

                if (countDelete == 1)
                {
                    // Restore stock in Book table
                    var sqlUpdate = "update Book set Quantity = Quantity + " + quantity + " where ISBN = '" + isbn + "';";
                    this.Da.ExecuteDMLQuery(sqlUpdate);

                    //MessageBox.Show("Book removed from cart and stock restored.");

                    this.PopulateGridView();      
                    this.PopulateCartGridView(); 

                    this.CalculateCartTotal();
                }
                else
                {
                    MessageBox.Show("Failed to remove book from cart.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCart.CurrentRow == null)
                {
                    MessageBox.Show("Please select a cart item to update first.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                // cart details
                int cartId = Convert.ToInt32(dgvCart.CurrentRow.Cells["Id"].Value);
                string isbn = txtISBN.Text;
                int oldQuantity = Convert.ToInt32(dgvCart.CurrentRow.Cells["Quantity"].Value);
                int newQuantity = Convert.ToInt32(txtQuantity.Text);
                decimal price = Convert.ToDecimal(txtPrice.Text);

                // quantity difference
                int quantityDifference = newQuantity - oldQuantity;

                // If no change, do nothing
                if (quantityDifference == 0)
                {
                    MessageBox.Show("No quantity change detected.");
                    return;
                }

                // Check if we need more books from inventory
                if (quantityDifference > 0)
                {
                    // Check available stock
                    var stockQuery = "select Quantity from Book where ISBN = '" + isbn + "';";
                    var dt = this.Da.ExecuteQueryTable(stockQuery);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Book not found in stock.");
                        return;
                    }

                    int availableStock = Convert.ToInt32(dt.Rows[0]["Quantity"]);
                    if (quantityDifference > availableStock)
                    {
                        MessageBox.Show("Not enough stock available. Only " + availableStock + " additional copies available.");
                        return;
                    }
                }

                // Update the cart with new quantity and total price
                decimal newTotalPrice = newQuantity * price;
                string updateCartSql = "update Cart set Quantity = " + newQuantity +
                                      ", TotalPrice = " + newTotalPrice +
                                      " where Id = " + cartId + ";";

                int cartUpdateResult = this.Da.ExecuteDMLQuery(updateCartSql);

                if (cartUpdateResult == 1)
                {
                    // Update book inventory
                    string updateBookSql = "update Book set Quantity = Quantity - (" + quantityDifference +
                                          ") where ISBN = '" + isbn + "';";
                    this.Da.ExecuteDMLQuery(updateBookSql);

                    MessageBox.Show("Cart updated successfully.");

                    // Refresh displays
                    this.PopulateGridView();
                    this.PopulateCartGridView();
                    this.CalculateCartTotal();
                    FormClear.ClearAllControls(this);
                }
                else
                {
                    MessageBox.Show("Failed to update cart.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating cart: " + ex.Message);
            }
        }

        private void dgvCart_DoubleClick(object sender, EventArgs e)
        {
            this.txtISBN.Text = this.dgvCart.CurrentRow.Cells[1].Value.ToString();
            this.txtName.Text = this.dgvCart.CurrentRow.Cells[2].Value.ToString();
            this.txtQuantity.Text = this.dgvCart.CurrentRow.Cells[3].Value.ToString();
            this.txtPrice.Text = this.dgvCart.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnBookRquest_Click(object sender, EventArgs e)
        {
            BookRequestPanel bookRequest = new BookRequestPanel();
            bookRequest.Show();
        }
    }
}
