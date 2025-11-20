using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lab_10
{
    public partial class Form1 : Form
    {
        private SqlDataAdapter adapter;
        private DataSet ds;
        private BindingSource bs;

        // Update this if your LocalDB or database is different
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Student\source\repos\Lab_10\StudentDB.mdf;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                ds = new DataSet();
                bs = new BindingSource();

                string sql = "SELECT StudentID, Name, Age FROM Student";
                adapter = new SqlDataAdapter(sql, connectionString);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                adapter.Fill(ds, "Student");
                bs.DataSource = ds.Tables["Student"];

                dgvStudents.DataSource = bs;

                if (dgvStudents.Columns["StudentID"] != null)
                    dgvStudents.Columns["StudentID"].ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text.Trim(), out int age))
            {
                MessageBox.Show("Please enter a valid age (integer).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow newRow = ds.Tables["Student"].NewRow();
            newRow["Name"] = name;
            newRow["Age"] = age;
            ds.Tables["Student"].Rows.Add(newRow);

            bs.MoveLast();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
            {
                MessageBox.Show("Select a student row to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView drv = bs.Current as DataRowView;
            if (drv != null)
            {
                string name = txtName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtAge.Text.Trim(), out int age))
                {
                    MessageBox.Show("Please enter a valid age (integer).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                drv["Name"] = name;
                drv["Age"] = age;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
            {
                MessageBox.Show("Select a row to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete the selected student?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                bs.RemoveCurrent();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bs.EndEdit();
                adapter.Update(ds, "Student");
                MessageBox.Show("Changes saved to database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving to database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (bs.Current != null)
            {
                DataRowView drv = bs.Current as DataRowView;
                if (drv != null)
                {
                    txtName.Text = drv["Name"].ToString();
                    txtAge.Text = drv["Age"].ToString();
                }
            }
        }
    }
}
