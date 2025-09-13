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
    public partial class EmployeePage : UserControl
    {
        private DataAccess Da { get; set; }
        public EmployeePage()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();
        }

        private void PopulateGridView(string sql = "select * from Employee;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvEmployee.AutoGenerateColumns = false;
            this.dgvEmployee.DataSource = ds.Tables[0];
        }

        private bool IsValidToSave()
        {
            if (string.IsNullOrEmpty(this.txtName.Text) ||
                string.IsNullOrEmpty(this.txtID.Text) ||
                string.IsNullOrEmpty(this.txtPassword.Text) ||
                string.IsNullOrEmpty(this.cmbActiveStatus.Text) ||
                string.IsNullOrEmpty(this.cmbRole.Text))
               
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*private void ClearAll()
        {
            this.txtID.Clear();
            this.txtName.Clear();
            this.txtPassword.Clear();
            this.cmbRole.SelectedIndex = -1;
            this.cmbActiveStatus.SelectedIndex = -1;

            this.dgvEmployee.ClearSelection();
        }*/

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the empty fields");
                    return;
                }

                var sql = "insert into Employee (ID, Name, Password, Role, ActiveStatus) " +
                          "values ('" + this.txtID.Text + "', '" + this.txtName.Text +
                          "', '" + this.txtPassword.Text + "', '" + this.cmbRole.Text +
                          "', '" + this.cmbActiveStatus.Text + "');";

                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show("Employee has been added successfully");
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidToSave())
                {
                    MessageBox.Show("Please fill all the empty fields");
                    return;
                }

                var sql = "update Employee " +
                          "set Name = '" + this.txtName.Text +
                          "', Password = '" + this.txtPassword.Text +
                          "', Role = '" + this.cmbRole.Text +
                          "', ActiveStatus = '" + this.cmbActiveStatus.Text +
                          "' where ID = '" + this.txtID.Text + "';";

                var count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show("Employee has been updated successfully");
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

        private void dgvEmployee_DoubleClick(object sender, EventArgs e)
        {
            this.txtName.Text = this.dgvEmployee.CurrentRow.Cells[0].Value.ToString();
            this.txtID.Text = this.dgvEmployee.CurrentRow.Cells[1].Value.ToString();
            this.txtPassword.Text = this.dgvEmployee.CurrentRow.Cells[2].Value.ToString();
            this.cmbRole.Text = this.dgvEmployee.CurrentRow.Cells[3].Value.ToString();
            this.cmbActiveStatus.Text = this.dgvEmployee.CurrentRow.Cells[4].Value.ToString();

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var sql = "select * from Employee where Name like '" + this.txtSearch.Text + "%' OR ID like '" + this.txtSearch.Text + "%' OR ActiveStatus like '" + this.txtSearch.Text + "%';";
            this.PopulateGridView(sql);
        }
    }
}
