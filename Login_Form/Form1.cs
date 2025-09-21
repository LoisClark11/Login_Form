using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_Form
{
    public partial class Form1 : Form
    {
        string connection = "Server=localhost;Database=loisdb;User ID=root;Password=LoiSQL@11;";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txt_Username.Focus();
            txt_Username.TextChanged += InputFields_TextChanged;
            txt_Password.TextChanged += InputFields_TextChanged;
            
        }
        private void InputFields_TextChanged(object sender, EventArgs e)
        {
            bool bothFilled = !string.IsNullOrWhiteSpace(txt_Username.Text)
                           && !string.IsNullOrWhiteSpace(txt_Password.Text);

            btn_LogIn.Enabled = bothFilled;
        }
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }

        private void btn_LogIn_Click(object sender, EventArgs e)
        {
            string username = txt_Username.Text.Trim();
            string password = txt_Password.Text;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT Name, password FROM users WHERE username = @username";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string name = reader["Name"].ToString();
                                string storedHash = reader["password"].ToString();

                                if (VerifyPassword(password, storedHash))
                                {
                                    MessageBox.Show("Hello, " + name + "!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Hide();
                                    Form2 form2 = new Form2();
                                    form2.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Wrong Username/Password", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    txt_Password.Clear();
                                    txt_Username.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txt_Username.Clear();
                                txt_Password.Clear();
                                txt_Username.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_LogIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_LogIn.PerformClick();
            }
        }
        private void txt_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btn_LogIn.PerformClick();
            }
        }

        private void connectionTest_Click_1(object sender, EventArgs e)
        {

        }

        private void connectionTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connection))
                {
                    conn.Open();
                    MessageBox.Show("Connection Successful!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed: " + ex.Message);
            }
        }

        private void txt_Username_Enter(object sender, EventArgs e)
        {
            txt_Username.BackColor = Color.White;
        }

        private void txt_Username_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Username.Text))
            {
                txt_Username.BackColor = Color.Silver;
            }
        }

        private void txt_Username_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_Password_Enter(object sender, EventArgs e)
        {
            txt_Password.BackColor = Color.White;
        }

        private void txt_Password_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Password.Text))
            {
                txt_Password.BackColor = Color.Silver;
            }
        }

        private void txt_Password_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
