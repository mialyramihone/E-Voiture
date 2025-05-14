using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFFAMMA.TECHNIQUE
{
    public partial class factureControl : UserControl
    {
        public factureControl()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=mlr1;Uid=root;Pwd=;");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void ajoutBtn_Click(object sender, EventArgs e)
        {

        }

        private void afficherBtn_Click(object sender, EventArgs e)
        {

        }

        private void majBtn_Click(object sender, EventArgs e)
        {

        }

        private void supBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
