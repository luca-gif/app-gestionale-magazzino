using System;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
            timer1.Start();
        }

        int starPoint = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            starPoint += 3;
            progressBar1.Value = starPoint;
            if (progressBar1.Value == 99)
            {
                progressBar1.Value = 0;
                timer1.Stop();
                LoginForms login = new LoginForms();
                this.Hide();
                login.ShowDialog();
            }
        }
    }
}
