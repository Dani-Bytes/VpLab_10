using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lab10_2
{
    public partial class Form1 : Form
    {
        string connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\StudentDB.mdf;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAllStudents();
        }

        private void LoadAllStudents()
        {
            string query = @"
                SELECT 
                    S.StudentID,
                    S.FirstName,
                    S.LastName,
                    S.Major,
                    D.DeptName,
                    D.DeptChair
                FROM Student S
                INNER JOIN Department D
                    ON S.DeptID = D.DeptID";

            LoadData(query);
        }

        private void LoadData(string query, SqlParameter parameter = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameter != null)
                        cmd.Parameters.Add(parameter);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvJoin.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data:\n" + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("Enter a last name to search.");
                return;
            }

            string query = @"
                SELECT 
                    S.StudentID,
                    S.FirstName,
                    S.LastName,
                    S.Major,
                    D.DeptName,
                    D.DeptChair
                FROM Student S
                INNER JOIN Department D
                    ON S.DeptID = D.DeptID
                WHERE S.LastName = @LastName";

            SqlParameter p = new SqlParameter("@LastName", txtLastName.Text.Trim());

            LoadData(query, p);
        }

        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            LoadAllStudents();
        }
    }
}
