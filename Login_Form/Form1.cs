using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.BackColor = Color.LightGreen;
            }
            else
            {
                textBox1.BackColor = Color.White;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "password")
            {
                MessageBox.Show("Login successful!");
                this.Hide();
                Form2 form2 = new Form2();
                form2.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                textBox2.BackColor = Color.LightGreen;
            }
            else
            {
                textBox2.BackColor = Color.White;
            }
        }
    }
}
