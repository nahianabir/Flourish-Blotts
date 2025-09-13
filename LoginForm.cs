using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Flourish___Blotts
{
    public partial class LoginForm : Form
    {
        private DataAccess Da { get; set; }
        public LoginForm()
        {
            InitializeComponent();
            this.Da = new DataAccess();
        }


        private void LoginForm_Load_1(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;

        }


        //Show pass
        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }




        public void ShowPanel(Form panelForm)
        {
            // Hide this login form instead of closing it
            this.Hide();

            // Set up the panel form to show the login form again on close
            panelForm.FormClosed += (s, args) => this.Show();

            
            panelForm.Show();
            FormClear.ClearAllControls(this);
        }







        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userID = txtID.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both Id and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                //admin login
                string adminsql = "SELECT * FROM Admin WHERE ID = '" + userID +
                             "' AND Password COLLATE SQL_Latin1_General_CP1_CS_AS = '" + password + "';";

                var adminDt = this.Da.ExecuteQueryTable(adminsql);

                if (adminDt.Rows.Count == 1)
                {
                    string adminName = adminDt.Rows[0]["Name"].ToString();
                    MessageBox.Show($"Admin login successful! Welcome, {adminName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    AdminPanelForm adminPanel = new AdminPanelForm();
                    
                    ShowPanel(adminPanel);
                    return;
                }





                //employee login
                string empSql = "SELECT * FROM Employee WHERE Id = '" + userID +
                        "' AND Password COLLATE SQL_Latin1_General_CP1_CS_AS = '" + password + "';";

                var empDt = this.Da.ExecuteQueryTable(empSql);

                if (empDt.Rows.Count == 1)
                {
                    string status = empDt.Rows[0]["ActiveStatus"].ToString();

                    if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    {
                        string empName = empDt.Rows[0]["Name"].ToString();
                        MessageBox.Show($"Employee login successful! Welcome, {empName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SalesmanPanelForm salesmanPanel = new SalesmanPanelForm();
                        salesmanPanel.LoggedInSalesmanID = userID;

                        ShowPanel(salesmanPanel);


                    }
                    else
                    {
                        MessageBox.Show("Your account is inactive. Contact admin.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ID or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
