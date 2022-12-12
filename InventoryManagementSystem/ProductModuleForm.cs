using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class ProductModuleForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luca8\OneDrive\Documenti\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();

        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCat.Items.Clear();
            cmd = new SqlCommand("SELECT name FROM tbCategory", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboCat.Items.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close();
        }

        private void pictureBoxClose_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("INSERT INTO tbProduct (name, quantity, price, description, category) VALUES (@name, @quantity, @price, @description, @category)", conn);
                    cmd.Parameters.Add("@name", textProductName.Text);
                    cmd.Parameters.Add("@quantity", Convert.ToInt16(textQuantity.Text));
                    cmd.Parameters.Add("@price", Convert.ToDouble(textPrice.Text));
                    cmd.Parameters.Add("@description", textDescription.Text);
                    cmd.Parameters.Add("@category", comboCat.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Product has been succesfully saved.");
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
            textProductName.Text = string.Empty;
            textDescription.Text = string.Empty;
            textPrice.Text = string.Empty;
            textQuantity.Text = string.Empty;
            comboCat.Text = string.Empty;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand("UPDATE tbProduct SET name = @name, description = @description, price = @price, quantity = @quantity, category = @category WHERE id LIKE @id", conn);
                    cmd.Parameters.Add("id", lblProductId.Text);
                    cmd.Parameters.Add("@name", textProductName.Text);
                    cmd.Parameters.Add("@description", textDescription.Text);
                    cmd.Parameters.Add("@price", Convert.ToDouble(textPrice.Text));
                    cmd.Parameters.Add("@quantity", Convert.ToInt16(textQuantity.Text));
                    cmd.Parameters.Add("@category", comboCat.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Product has been succesfully updated.");
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
