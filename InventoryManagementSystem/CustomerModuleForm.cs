using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class CustomerModuleForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luca8\OneDrive\Documenti\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");

        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this customer?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbCustomer (customer_name, customer_lastname, customer_phone, customer_city, customer_address) VALUES (@name, @lastname, @phone, @city, @address)", conn);
                    cmd.Parameters.Add("@name", textName.Text);
                    cmd.Parameters.Add("@lastname", textLastname.Text);
                    cmd.Parameters.Add("@phone", textPhone.Text);
                    cmd.Parameters.Add("@city", textCity.Text);
                    cmd.Parameters.Add("@address", textAddress.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Customer has been succesfully saved.");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            textName.Text = string.Empty;
            textLastname.Text = string.Empty;
            textPhone.Text = string.Empty;
            textCity.Text = string.Empty;
            textAddress.Text = string.Empty;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this customer?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE tbCustomer SET customer_name = @name, customer_lastname = @lastname, customer_phone = @phone, customer_city = @city, customer_address = @address WHERE customer_id LIKE '" + lblCustomerId.Text + "'", conn);
                    cmd.Parameters.Add("@name", textName.Text);
                    cmd.Parameters.Add("@lastname", textLastname.Text);
                    cmd.Parameters.Add("@phone", textPhone.Text);
                    cmd.Parameters.Add("@city", textCity.Text);
                    cmd.Parameters.Add("@address", textAddress.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Customer has been succesfully updated.");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
