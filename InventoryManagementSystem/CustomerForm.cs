using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem
{

    public partial class CustomerForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luca8\OneDrive\Documenti\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();

        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        public void LoadCustomers()
        {
            try
            {
                dgvCustomer.Rows.Clear();
                cmd = new SqlCommand("SELECT * FROM tbCustomer", conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[5].ToString(), reader[4].ToString());
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModuleForm moduleForm = new CustomerModuleForm();
            moduleForm.btnSave.Enabled = true;
            moduleForm.btnUpdate.Enabled = false;
            moduleForm.ShowDialog();
            LoadCustomers();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                CustomerModuleForm customerModule = new CustomerModuleForm();
                customerModule.lblCustomerId.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                customerModule.textName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                customerModule.textLastname.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                customerModule.textPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                customerModule.textCity.Text = dgvCustomer.Rows[e.RowIndex].Cells[5].Value.ToString();
                customerModule.textAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[6].Value.ToString();

                customerModule.btnSave.Enabled = false;
                customerModule.btnUpdate.Enabled = true;
                customerModule.ShowDialog();

            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this customer?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM tbCustomer WHERE customer_id LIKE '" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been succesfully deleted!");
                }
            }

            LoadCustomers();
        }
    }
}
