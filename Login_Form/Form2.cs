using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Login_Form
{
    public partial class Form2 : Form
    {
        string connectionString = "Server=localhost;Database=loisdb;User ID=root;Password=LoiSQL@11;";

        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
        
        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM users";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtbx_ID.Text = row.Cells["id"].Value.ToString();
                txtbx_Name.Text = row.Cells["name"].Value.ToString();
                txtbx_Age.Text = row.Cells["age"].Value.ToString();
                txtbx_Email.Text = row.Cells["email"].Value.ToString();
                txtbx_Username.Text = row.Cells["username"].Value.ToString();
                txtbx_Password.Text = row.Cells["password"].Value.ToString();
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO users (name, age, email, username, password) VALUES (@name, @age, @email, @username, @password)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtbx_Name.Text);
                        cmd.Parameters.AddWithValue("@age", txtbx_Age.Text);
                        cmd.Parameters.AddWithValue("@email", txtbx_Email.Text);
                        cmd.Parameters.AddWithValue("@username", txtbx_Username.Text);
                        cmd.Parameters.AddWithValue("@password", txtbx_Password.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (string.IsNullOrWhiteSpace(txtbx_Name.Text) ||
                            string.IsNullOrWhiteSpace(txtbx_Age.Text) ||
                            string.IsNullOrWhiteSpace(txtbx_Email.Text) ||
                            string.IsNullOrWhiteSpace(txtbx_Username.Text) ||
                            string.IsNullOrWhiteSpace(txtbx_Password.Text))
                        {
                            MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User added successfully!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                if (string.IsNullOrWhiteSpace(txtbx_ID.Text))
                {
                    MessageBox.Show("Please enter an ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(txtbx_ID.Text, out int id))
                {
                    MessageBox.Show("ID must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    conn.Open();
                    string backupQuery = "INSERT INTO users_backup (id, name, age, email, username, password) " +
                                 "SELECT id, name, age, email, username, password FROM users WHERE id = @id";
                    using (MySqlCommand backupCmd = new MySqlCommand(backupQuery, conn))
                    {
                        backupCmd.Parameters.AddWithValue("@id", id);
                        backupCmd.ExecuteNonQuery();
                    }

                    string deletequery = "DELETE FROM users WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(deletequery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User deleted successfully!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete user. Please check the ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void btn_Update_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE users SET id = @id, name = @name, age = @age, email = @email, username = @username, password = @password WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtbx_ID.Text);
                    cmd.Parameters.AddWithValue("@name", txtbx_Name.Text);
                    cmd.Parameters.AddWithValue("@age", txtbx_Age.Text);
                    cmd.Parameters.AddWithValue("@email", txtbx_Email.Text);
                    cmd.Parameters.AddWithValue("@username", txtbx_Username.Text);
                    cmd.Parameters.AddWithValue("@password", txtbx_Password.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User updated successfully!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }    
        }

        private void btn_View_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = !dataGridView1.Enabled;

            if (dataGridView1.Enabled)
            {
                LoadData();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }
    }
}
