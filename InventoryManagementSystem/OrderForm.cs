using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class OrderForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luca8\OneDrive\Documenti\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();

        public OrderForm()
        {
            InitializeComponent();
            LoadOrders();
        }

        public void LoadOrders()
        {
            try
            {
                dgvOrder.Rows.Clear();

                string orderQuery = "SELECT o.order_id, o.order_date, o.product_id, p.name, o.customer_id, c.customer_name, c.customer_lastname, o.price, o.quantity, o.total FROM tbOrder AS o JOIN tbCustomer AS c ON o.customer_id = c.customer_id " +
                    "JOIN tbProduct AS p ON p.id = o.Product_id WHERE p.name LIKE '%" + textSearchOrder.Text + "%'";

                cmd = new SqlCommand(orderQuery, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int i = 0;

                while (reader.Read())
                {
                    i++;
                    dgvOrder.Rows.Add(i, reader[0].ToString(), Convert.ToDateTime(reader[1].ToString()).ToString("dd/MM/yyyy"), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString());
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
            OrderModuleForm moduleForm = new OrderModuleForm();
            moduleForm.btnInsert.Enabled = true;
            moduleForm.ShowDialog();
            LoadOrders();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrder.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this order?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM tbOrder WHERE order_id LIKE '" + dgvOrder.Rows[e.RowIndex].Cells[1].Value + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    cmd = new SqlCommand("UPDATE tbProduct SET quantity = (quantity + @quantity) WHERE id LIKE '" + dgvOrder.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", conn);
                    cmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(dgvOrder.Rows[e.RowIndex].Cells[5].Value.ToString()));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Record has been succesfully deleted!");
                }
            }
            LoadOrders();
        }

        private void textSearchOrder_TextChanged(object sender, EventArgs e)
        {
            LoadOrders();
        }
    }
}
