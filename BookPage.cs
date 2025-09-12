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
    public partial class BookPage : UserControl
    {

        private DataAccess Da { get; set; }
        public BookPage()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();

        }


        private void PopulateGridView(string sql = "select * from Book;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvBook.AutoGenerateColumns = false;
            this.dgvBook.DataSource = ds.Tables[0];
        }

        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            this.txtISBN.Text = this.dgvBook.CurrentRow.Cells[0].Value.ToString();
            this.txtName.Text = this.dgvBook.CurrentRow.Cells[1].Value.ToString();
            this.txtAuthorName.Text = this.dgvBook.CurrentRow.Cells[2].Value.ToString();
            this.cmbGenre.Text = this.dgvBook.CurrentRow.Cells[3].Value.ToString();
            this.dtpPublishingYear.Text = this.dgvBook.CurrentRow.Cells[4].Value.ToString();
            this.txtQuantity.Text = this.dgvBook.CurrentRow.Cells[5].Value.ToString();
            this.txtPrice.Text = this.dgvBook.CurrentRow.Cells[6].Value.ToString();
        }
        
        
        private bool IsValidToSave()
        {
            if (string.IsNullOrEmpty(this.txtISBN.Text) || 
                string.IsNullOrEmpty(this.txtName.Text) ||
                string.IsNullOrEmpty(this.txtAuthorName.Text) || 
                string.IsNullOrEmpty(this.cmbGenre.Text) ||
                string.IsNullOrEmpty(this.dtpPublishingYear.Text) || 
                string.IsNullOrEmpty(this.txtQuantity.Text) ||
                string.IsNullOrEmpty(this.txtPrice.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


       /* private void ClearAll()
        {
            this.txtISBN.Clear();
            this.txtName.Clear();
            this.txtAuthorName.Clear();
            this.cmbGenre.SelectedIndex = -1;
            this.dtpPublishingYear.Text = "";
            this.txtQuantity.Clear();
            this.txtPrice.Clear();
            

            
            this.txtAutoSearch.Text = "";

            this.dgvBook.ClearSelection();
        }*/


        //add
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the empty fields");
                    return;
                }

                var sql = "insert into Book (ISBN, Name, AuthorName, Genre, PublishingYear, Quantity, Price) " +
                          "values ('" + this.txtISBN.Text + "', '" + this.txtName.Text + "', '" + this.txtAuthorName.Text +
                          "', '" + this.cmbGenre.Text + "', '" + this.dtpPublishingYear.Text + "', " + this.txtQuantity.Text +
                          ", " + this.txtPrice.Text + ");";

                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show("Data has been added successfully");
                else
                    MessageBox.Show("Insert failed.");

                this.PopulateGridView();
                FormClear.ClearAllControls(this);
                //this.ClearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //update
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the empty fields");
                    return;
                }

                var sql = "update Book " +
                          "set Name = '" + this.txtName.Text +
                          "', AuthorName = '" + this.txtAuthorName.Text +
                          "', Genre = '" + this.cmbGenre.Text +
                          "', PublishingYear = '" + this.dtpPublishingYear.Text +
                          "', Quantity = " + this.txtQuantity.Text +
                          ", Price = " + this.txtPrice.Text +
                          " where ISBN = '" + this.txtISBN.Text + "';";

                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show("Data has been updated successfully");
                else
                    MessageBox.Show("Update failed. No matching record found.");

                this.PopulateGridView();
                FormClear.ClearAllControls(this);
                //this.ClearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BookPage_Load(object sender, EventArgs e)
        {
            this.dgvBook.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBook.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a row first to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                var isbn = this.dgvBook.CurrentRow.Cells[0].Value.ToString();
                var name = this.dgvBook.CurrentRow.Cells[1].Value.ToString();

                DialogResult res = MessageBox.Show("Are you sure to remove " + name + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No)
                    return;

                var sql = "delete from Book where ISBN = '" + isbn + "';";
                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show(name.ToUpper() + " has been removed from the list");
                else
                    MessageBox.Show("Data hasn't been deleted");

                this.PopulateGridView();
                FormClear.ClearAllControls(this);
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
            }
        }


        private void SearchBook()
        {
            var sql = "select * from Book where Name like '" + this.txtAutoSearch.Text + "%';";
            this.PopulateGridView(sql);
        }
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SearchBook();
        }

        private void txtAutoSearch_TextChanged_1(object sender, EventArgs e)
        {
            this.SearchBook();
        }
    }
}
