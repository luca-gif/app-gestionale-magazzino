using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class OrderModuleForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luca8\OneDrive\Documenti\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();

        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomers();
            LoadProduct();
        }

        public void LoadCustomers()
        {
            try
            {
                dgvCustomer.Rows.Clear();
                cmd = new SqlCommand("SELECT customer_id, customer_name FROM tbCustomer WHERE customer_name LIKE '%" + textSearchCust.Text + "%'", conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, reader[0].ToString(), reader[1].ToString());
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cmd = new SqlCommand("SELECT * FROM tbProduct WHERE name LIKE '%" + txtSearchProd.Text + "%'", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[4].ToString(), reader[3].ToString(), reader[2].ToString(), reader[5].ToString());
            }

            reader.Close();
            conn.Close();
        }

        private void textSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void txtSearchProd_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void pictureBoxClose_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        double qty = 0;

        private void textProductQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(textProductQuantity.Value) > qty)
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textProductQuantity.Value = textProductQuantity.Value - 1;
                return;
            }

            try
            {
                if (Convert.ToInt32(textProductQuantity.Value) > 0)
                {
                    double total = Convert.ToDouble(textProductPrice.Text) * Convert.ToDouble(textProductQuantity.Value);
                    textProductTotal.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textCustomerId.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            textCustomerName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textProductId.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            textProductName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            textProductPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
            qty = Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString());
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (textCustomerId.Text == string.Empty)
                {
                    MessageBox.Show("Please select customer!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textProductId.Text == string.Empty)
                {
                    MessageBox.Show("Please select product!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textProductQuantity.Value.Equals(0))
                {
                    MessageBox.Show("Please select a quantity!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to insert this order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("INSERT INTO tbOrder (order_date, product_id, customer_id, quantity, price, total) VALUES (@order_date, @product_id, @customer_id, @quantity, @price, @total)", conn);
                    cmd.Parameters.Add("@order_date", dateTimePicker1.Value);
                    cmd.Parameters.Add("@product_id", Convert.ToInt32(textProductId.Text));
                    cmd.Parameters.Add("@customer_id", Convert.ToInt32(textCustomerId.Text));
                    cmd.Parameters.Add("@quantity", Convert.ToInt32(textProductQuantity.Value));
                    cmd.Parameters.Add("@price", Convert.ToDouble(textProductPrice.Text));
                    cmd.Parameters.Add("@total", Convert.ToDouble(textProductTotal.Text));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();


                    cmd = new SqlCommand("UPDATE tbProduct SET quantity = (quantity - @quantity) WHERE id LIKE '" + textProductId.Text + "'", conn);
                    cmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(textProductQuantity.Value));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadProduct();

                    MessageBox.Show("Order has been succesfully inserted.");
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
            textProductId.Clear();
            textCustomerId.Clear();
            textCustomerName.Clear();
            textProductName.Clear();
            textProductPrice.Clear();
            textProductTotal.Clear();
            textProductQuantity.Value = 1;
            textProductTotal.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnInsert.Enabled = true;
        }


    }
}


