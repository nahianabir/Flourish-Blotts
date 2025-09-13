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
    public partial class HomePage : UserControl
    {
        private DataAccess Da { get; set; }
        public HomePage()
        {
            InitializeComponent();
            this.Da = new DataAccess();
            this.LoadHomeStats();
            this.LoadMonthlySalesChart();
        }


        private void LoadHomeStats()
        {
            string query = @"
           SELECT
               (SELECT SUM(TotalPrice) FROM Sales) AS TotalSale,
               (SELECT SUM(Quantity) FROM Sales) AS TotalBooksSold,
               (SELECT COUNT(*) FROM Book) AS TotalBooks,
               (SELECT COUNT(*) FROM Employee WHERE ActiveStatus = 'Active') AS ActiveSalesmen
            ";

            DataTable dt = this.Da.ExecuteQueryTable(query);
            dgvShow.DataSource = dt; // assign the DataTable to your DataGridView
        }




        private void LoadMonthlySalesChart()
        {
            string query = @"
        SELECT 
            FORMAT(SaleDate, 'yyyy-MM') AS SaleMonth,
            SUM(TotalPrice) AS TotalSale
            FROM Sales
            GROUP BY FORMAT(SaleDate, 'yyyy-MM')
            ORDER BY SaleMonth
            ";

            DataTable dt = this.Da.ExecuteQueryTable(query);

            chtSales.Series.Clear();
            chtSales.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chtSales.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            var series = chtSales.Series.Add("Monthly Sales");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            foreach (DataRow row in dt.Rows)
            {
                series.Points.AddXY(row["SaleMonth"], row["TotalSale"]);
            }
        }





    }
}
