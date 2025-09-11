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
        private string text;
        private int len = 0;
        private bool isFirstAnimationComplete = false;
        private string welcomeText;
        private string backText;
        public LoginForm()
        {
            InitializeComponent();
            this.Da = new DataAccess();
        }

        

        //text animation
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (!isFirstAnimationComplete)
            {
                // (lblWelcome)
                if (len < text.Length)
                {
                    lblWelcome.Text = lblWelcome.Text + text.ElementAt(len);
                    len++;
                }
                else
                {
                    // First animation completed
                    isFirstAnimationComplete = true;
                    len = 0;
                    text = backText;

                    // Make second label visible and start its animation
                    lblBack.Visible = true;
                    lblBack.Text = "";
                }
            }
            else
            {
                //(lblBack)
                if (len < text.Length)
                {
                    lblBack.Text = lblBack.Text + text.ElementAt(len);
                    len++;
                }
                else
                {
                    timer1.Stop();
                }
            }
        }

        private void LoginForm_Load_1(object sender, EventArgs e)
        {
            // Save original texts
            welcomeText = lblWelcome.Text;
            backText = lblBack.Text;

            // Hide second label initially
            lblBack.Visible = false;

            // Start first animation
            text = welcomeText;
            lblWelcome.Text = "";
            timer1.Start();
        }


        //Show pass
        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = true;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
            }
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
                // ❗ Using your DataAccess.ExecuteQueryTable
                string sql = "SELECT * FROM Admin WHERE ID = '" + userID +
                             "' AND Password COLLATE SQL_Latin1_General_CP1_CS_AS = '" + password + "';";

                var dt = this.Da.ExecuteQueryTable(sql);

                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 👉 Open AdminPanelForm
                    AdminPanelForm adminPanel = new AdminPanelForm();
                    adminPanel.Show();
                    this.Hide();
                    return;
                    adminPanel.FormClosed += (s, args) => this.Close();
                }

                //employee login

                string empSql = "SELECT * FROM Salesman WHERE Id = '" + userID +
                        "' AND Password COLLATE SQL_Latin1_General_CP1_CS_AS = '" + password + "';";

                var empDt = this.Da.ExecuteQueryTable(empSql);

                if (empDt.Rows.Count == 1)
                {
                    MessageBox.Show("Salesman login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SalesmanPanelForm salesmanPanel = new SalesmanPanelForm();
                    salesmanPanel.Show();
                    this.Hide();

                    //need to be fixed
                    salesmanPanel.FormClosed += (s, args) => this.Close();
                    return;
                }

                // 🔹 Step 3: Neither Admin nor Employee
                MessageBox.Show("Invalid ID or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
