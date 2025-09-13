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
    public partial class RequestsPage : UserControl
    {
        private DataAccess Da { get; set; }
        public RequestsPage()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.PopulateGridView();
        }

        private void PopulateGridView(string sql = "select * from BookRequests;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvBookReq.AutoGenerateColumns = false;
            this.dgvBookReq.DataSource = ds.Tables[0];
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvBookReq.SelectedRows.Count == 0 && dgvBookReq.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a book request to delete.", "No Selection",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected row
            DataGridViewRow row;
            if (dgvBookReq.SelectedRows.Count > 0)
                row = dgvBookReq.SelectedRows[0];
            else
                row = dgvBookReq.Rows[dgvBookReq.SelectedCells[0].RowIndex];

            // Assuming the first column contains the ID of the request
            var requestId = Convert.ToInt32(row.Cells[0].Value);

            // Ask for confirmation
            DialogResult result = MessageBox.Show("Are you sure you want to delete this book request?","Confirm Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Execute the delete operation
                string sql = $"DELETE FROM BookRequests WHERE RequestID = {requestId}";
                var deleted = this.Da.ExecuteDMLQuery(sql);

                if (deleted > 0)
                {
                    MessageBox.Show("Book request deleted successfully.", "Success",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    this.PopulateGridView();
                }
                else
                {
                    MessageBox.Show("Failed to delete the book request.", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
