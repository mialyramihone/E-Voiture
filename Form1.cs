using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFFAMMA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void chargement_timer_Tick(object sender, EventArgs e)
        {
            if (chargement_Bar.Value < chargement_Bar.Maximum)
            {

                chargement_Bar.Value += 1;
            }
            else
            {

                chargement_timer.Stop(); 
                Connexion newForm = new Connexion();
                //newForm.FormClosed += NewForm_FormClosed;
                newForm.Show();
                this.Hide();
            }
        }


        private void chargement_Bar_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            chargement_Bar.Maximum = 100; 
            chargement_Bar.Value = 0;
            chargement_timer.Interval = 30;
            //chargement_timer.Interval = 100;
            chargement_timer.Start();
        }
    }
}
