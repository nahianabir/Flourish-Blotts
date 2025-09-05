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
    public partial class LoginForm : Form
    {

        private string text;
        private int len = 0;
        private bool isFirstAnimationComplete = false;
        private string welcomeText;
        private string backText;
        public LoginForm()
        {
            InitializeComponent();
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

        

    }
}
