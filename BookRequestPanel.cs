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
    public partial class BookRequestPanel : Form
    {
        private DataAccess Da { get; set; }
        public BookRequestPanel()
        {
            InitializeComponent();
            this.Da = new DataAccess();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtBookName.Text) ||
                string.IsNullOrWhiteSpace(this.txtAuthorName.Text))
            {
                MessageBox.Show("Please enter both Book Name and Author Name.",
                                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            try
            {
                string bookName = txtBookName.Text.Trim();
                string authorName = txtAuthorName.Text.Trim();
                string insertSql = $"INSERT INTO BookRequests (BookName, AuthorName) VALUES ('{bookName}', '{authorName}')";

                int result = this.Da.ExecuteDMLQuery(insertSql);

                if (result > 0)
                {
                    MessageBox.Show("Book request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FormClear.ClearAllControls(this);

                }
                else
                {
                    MessageBox.Show("Failed to submit book request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
