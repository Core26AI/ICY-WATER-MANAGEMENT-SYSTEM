using ICY_ICY_WATER.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICY_ICY_WATER
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            loadGrossProfit();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnDashboard.Height;
            panelSlide.Top = btnDashboard.Top;
            

        }

        private void btnEmployer_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnEmployer.Height;
            panelSlide.Top = btnEmployer.Top;
            openChildForm(new Employer());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnCustomer.Height;
            panelSlide.Top = btnCustomer.Top;
            openChildForm(new Customer());
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnService.Height;
            panelSlide.Top = btnService.Top;
            openChildForm(new Service());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnCash.Height;
            panelSlide.Top = btnCash.Top;
            openChildForm(new Cash(this));
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnReport.Height;
            panelSlide.Top = btnReport.Top;
            openChildForm(new Report());
            
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnSettings.Height;
            panelSlide.Top = btnSettings.Top;
            openChildForm(new Settings());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnLogout.Height;
            panelSlide.Top = btnLogout.Top;
        }
        #region method
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void loadGrossProfit()
        {
            Report module =new Report();
            lblRevenues.Text = module.extractData("SELECT ISNULL(SUM(CAST(price AS DECIMAL(18, 2))), 0) AS total FROM tbCash WHERE date >'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "' AND status LIKE 'Sold' ").ToString("#,##0.00");
            lblCostofGood.Text = module.extractData("SELECT ISNULL(SUM(CAST(cost AS DECIMAL(18, 2))), 0) AS Cost FROM tbCostofGood WHERE date > '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "'").ToString("#,##0.00");
            lblGrossProfit.Text = (double.Parse(lblRevenues.Text) - double.Parse(lblCostofGood.Text)).ToString("#,##0.00");
        
            double revlast7 = module.extractData("SELECT ISNULL(SUM(CAST(price AS DECIMAL(18, 2))), 0) AS total FROM tbCash WHERE date BETWEEN '" + DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd") + "'AND'"+ DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")+ "' AND status LIKE 'Sold' ");
            double coglast7 = module.extractData("SELECT ISNULL(SUM(CAST(cost AS DECIMAL(18, 2))), 0) AS Cost FROM tbCostofGood WHERE date BETWEEN '" + DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd") + "'AND'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "'");
            double gplast7 = revlast7 - coglast7;

            if (revlast7 > double.Parse(lblRevenues.Text))
                picRevenues.Image = Properties.Resources.Down;
            else
                picRevenues.Image = Properties.Resources.Up;

            if (gplast7 > double.Parse(lblGrossProfit.Text))
            {
                picGrossProfit.Image = Properties.Resources.Down;
                lblGrossProfit.ForeColor = Color.Red;
            }
            else
            {
                picGrossProfit.Image = Properties.Resources.Up;
                lblGrossProfit.ForeColor = Color.Green;
            }
        }

        #endregion method
    }
}
