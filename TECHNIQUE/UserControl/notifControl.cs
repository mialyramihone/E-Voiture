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
    public partial class notifControl : UserControl
    {
        public notifControl()
        {
            InitializeComponent();

            MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=mlr1;Uid=root;Pwd=;");

        }
    }
}
