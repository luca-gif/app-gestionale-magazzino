using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryManagementSystem
{

    public partial class CategoryForm : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luca8\OneDrive\Documenti\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");

        public CategoryForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tbCategory", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, reader[0].ToString(), reader[1].ToString());
            }
            reader.Close();
            conn.Close();
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            CategoryModelForm categoryModelForm = new CategoryModelForm();
            categoryModelForm.btnSave.Enabled = true;
            categoryModelForm.btnUpdate.Enabled = false;
            categoryModelForm.ShowDialog();
            LoadCategory();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                CategoryModelForm categoryModule = new CategoryModelForm();
                categoryModule.lblCategoryId.Text = dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                categoryModule.textCatName.Text = dgvCategory.Rows[e.RowIndex].Cells[2].Value.ToString();

                categoryModule.btnSave.Enabled = false;
                categoryModule.btnUpdate.Enabled = true;
                categoryModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM tbCategory WHERE category_id LIKE '" + dgvCategory.Rows[e.RowIndex].Cells[1].Value + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Record has been succesfully deleted!");
                }
            }

            LoadCategory();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }
    }
}
