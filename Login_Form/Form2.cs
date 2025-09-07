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

namespace Login_Form
{
    public partial class Form2 : Form
    {
        string connectionString = "Server=localhost;Database=loisdb;User ID=root;Password=LoiSQL@11;";

        public Form2()
        {
            InitializeComponent();
        }
        
        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM user";
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
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
