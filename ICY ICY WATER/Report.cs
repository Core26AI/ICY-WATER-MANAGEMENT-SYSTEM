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

namespace ICY_ICY_WATER
{
    public partial class Report : Form
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        string title = "Icy Water Management System";
        public Report()
        {
            InitializeComponent();
            loadTopSelling();
            loadRevenues();
            loadCostofGood();
            loadGrossProfit();
        }
        #region Top Selling
        private void dtFromTopSelling_ValueChanged(object sender, EventArgs e)
        {
            loadTopSelling();
        }
        private void dtToTopSelling_ValueChanged(object sender, EventArgs e)
        {
            loadTopSelling();
        }
        public void loadTopSelling() 
        {
            try
            {
                int i = 0;
                dgvTopSelling.Rows.Clear();
                string fromDate = dtFromTopSelling.Value.Date.ToString("yyyy-MM-dd");
                string toDate = dtFromTopSelling.Value.Date.AddDays(365).ToString("yyyy-MM-dd");
                cm = new SqlCommand("SELECT TOP 10 se.name,count(ca.sid) AS qty, ISNULL(SUM(ca.price),0) AS total FROM tbCash AS ca JOIN tbService AS se ON ca.sid=se.id WHERE ca.Date BETWEEN '"+fromDate+"' AND '"+toDate+"' AND status LIKE 'Sold' " +
                    "GROUP BY se.name ORDER BY qty DESC", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while(dr.Read()) 
                {
                    i++;
                    dgvTopSelling.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
            dr.Close();
            dbcon.close();
        }
        #endregion Top Selling
        //Top Selling Ends here

        #region Revenues
        private void dtFromRevenue_ValueChanged(object sender, EventArgs e)
        {
            loadRevenues();
        }

        private void dtToRevenue_ValueChanged(object sender, EventArgs e)
        {
            loadRevenues();
        }

        public void loadRevenues()
        {
            try
            {
                int i = 0;
                dgvRevenue.Rows.Clear();
                string fromDate = dtFromRevenue.Value.Date.ToString("yyyy-MM-dd");
                string toDate = dtToRevenue.Value.Date.AddDays(365).ToString("yyyy-MM-dd");
                double total = 0;
                cm = new SqlCommand("SELECT date,ISNULL(sum(price),0) AS total FROM tbCash WHERE date BETWEEN '" +fromDate + "' " +
                    "AND '" + toDate + "' AND status LIKE 'Sold' GROUP BY date", dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvRevenue.Rows.Add(i, DateTime.Parse(dr[0].ToString()).ToShortDateString(), dr[1].ToString());
                    total += double.Parse(dr[1].ToString());
                }
                lblRevenue.Text = total.ToString("#,##0.00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
            dr.Close();
            dbcon.close();
        }
        #endregion Revenues
        //Revenues ends here

        #region Cost of Good
        private void dtFromCoG_ValueChanged(object sender, EventArgs e)
        {
            loadCostofGood();
        }

        private void dtToCoG_ValueChanged(object sender, EventArgs e)
        {
            loadCostofGood();
        }
        public void loadCostofGood() 
        {
            try
            {
                int i = 0;
                dgvCoG.Rows.Clear();
                string fromDate = dtFromCoG.Value.Date.ToString("yyyy-MM-dd");
                string toDate = dtToCoG.Value.Date.AddDays(365).ToString("yyyy-MM-dd");
                double total = 0;
                cm = new SqlCommand("SELECT costname,cost,date FROM tbCostofGood WHERE date BETWEEN '" + fromDate + "' AND '" + toDate + "'",dbcon.connect());
                dbcon.open();
                dr = cm.ExecuteReader();
                while (dr.Read()) 
                {
                    i++;
                    dgvCoG.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), DateTime.Parse(dr[2].ToString()).ToShortDateString());
                    total += double.Parse(dr[1].ToString());
                }
                lblCoG.Text = total.ToString("#,##0.00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
            finally
            {
                dr.Close();
                dbcon.close();
            }
        }

        #endregion Cost of Good
        //Cost of Good ends here

        #region Gross Profit
        private void dtFromGP_ValueChanged(object sender, EventArgs e)
        {
            loadGrossProfit();
        }

        private void dtToGP_ValueChanged(object sender, EventArgs e)
        {
            loadGrossProfit();
        }
        public void loadGrossProfit() 
        {
            string fromDate = dtFromCoG.Value.Date.ToString("yyyy-MM-dd");
            string toDate = dtToCoG.Value.Date.AddDays(365).ToString("yyyy-MM-dd");
            txtRevenues.Text = extractData("SELECT ISNULL(SUM(CAST(price AS DECIMAL(18, 2))), 0) AS total FROM tbCash WHERE date BETWEEN '" + fromDate + "' AND '" + toDate + "'").ToString("#,##0.00");
            txtCostOfGood.Text = extractData("SELECT ISNULL(SUM(CAST(cost AS DECIMAL(18, 2))), 0) AS Cost FROM tbCostofGood WHERE date BETWEEN '" + fromDate + "' AND '" + toDate + "'").ToString("#,##0.00");
            txtGrossProfit.Text = (double.Parse(txtRevenues.Text)-double.Parse(txtCostOfGood.Text)).ToString("#,##0.00");
            if (double.Parse(txtGrossProfit.Text) < 0)
                txtGrossProfit.ForeColor = Color.Red;
            else
                txtGrossProfit.ForeColor = Color.Green;

        }

        public double extractData(string sql) 
        {
            dbcon.open();
            cm=new SqlCommand(sql,dbcon.connect());
            double data = double.Parse(cm.ExecuteScalar().ToString());
            dbcon.close();
            return data;
        }

        #endregion Gross Profit


    }
}
