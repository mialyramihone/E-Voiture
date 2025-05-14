using CFFAMMA.TECHNIQUE;
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
    public partial class Connexion : Form
    {
        public Connexion()
        {
            InitializeComponent();
            InitializePlaceholder();
            InitializePlaceholder1();
            CustomizeTableLayoutPanel1();
            CustomizeTableLayoutPanel12();

            connexionBtn.FlatStyle = FlatStyle.Flat;
            connexionBtn.BackColor = Color.FromArgb(60, 194, 98); 
            connexionBtn.ForeColor = Color.White;
            connexionBtn.FlatAppearance.BorderSize = 0;

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int cornerRadius = 20;
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(connexionBtn.Width - cornerRadius - 1, 0, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(connexionBtn.Width - cornerRadius - 1, connexionBtn.Height - cornerRadius - 1, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(0, connexionBtn.Height - cornerRadius - 1, cornerRadius, cornerRadius, 90, 90);
            path.CloseFigure();

            connexionBtn.Region = new Region(path);


            textBoxUsername.BorderStyle = BorderStyle.None; 
            textBoxUsername.BackColor = Color.White;        
            textBoxUsername.Font = new Font("Yu Gothic UI Semibold", 7);
            textBoxUsername.Padding = new Padding(5, 0, 0, 13);


            textBoxPassword.BorderStyle = BorderStyle.None;
            textBoxPassword.BackColor = Color.White;
            textBoxPassword.Font = new Font("Yu Gothic UI Semibold", 7);
            textBoxPassword.Padding = new Padding(5, 0, 0, 13);

            //Panel underline = new Panel();
            //underline.Height = 2;                           
            //underline.Dock = DockStyle.Bottom;            
            //underline.BackColor = Color.Green;              


            //textBoxUsername.Controls.Add(underline);


            //underline.BringToFront();



        }

        //tableLayoutPanel12
        private void CustomizeTableLayoutPanel1()
        {

            tableLayoutPanel11.BorderStyle = BorderStyle.None; 


            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int cornerRadius = 15; 


            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90); 
            path.AddArc(tableLayoutPanel11.Width - cornerRadius - 1, 0, cornerRadius, cornerRadius, 270, 90); // Coin supérieur droit
            path.AddArc(tableLayoutPanel11.Width - cornerRadius - 1, tableLayoutPanel11.Height - cornerRadius - 1, cornerRadius, cornerRadius, 0, 90); // Coin inférieur droit
            path.AddArc(0, tableLayoutPanel11.Height - cornerRadius - 1, cornerRadius, cornerRadius, 90, 90); // Coin inférieur gauche
            path.CloseFigure();

            tableLayoutPanel11.Region = new Region(path);

            tableLayoutPanel11.Paint += (sender, e) =>
            {
                e.Graphics.DrawPath(new Pen(Color.Green, 2), path);
            };
        }

        private void CustomizeTableLayoutPanel12()
        {

            tableLayoutPanel12.BorderStyle = BorderStyle.None;


            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int cornerRadius = 15; 


            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90); 
            path.AddArc(tableLayoutPanel12.Width - cornerRadius - 1, 0, cornerRadius, cornerRadius, 270, 90); // Coin supérieur droit
            path.AddArc(tableLayoutPanel12.Width - cornerRadius - 1, tableLayoutPanel12.Height - cornerRadius - 1, cornerRadius, cornerRadius, 0, 90); // Coin inférieur droit
            path.AddArc(0, tableLayoutPanel12.Height - cornerRadius - 1, cornerRadius, cornerRadius, 90, 90); // Coin inférieur gauche
            path.CloseFigure();


            tableLayoutPanel12.Region = new Region(path);


            tableLayoutPanel12.Paint += (sender, e) =>
            {
                e.Graphics.DrawPath(new Pen(Color.Green, 2), path); 
            };
        }




        private void InitializePlaceholder()
        {
            textBoxUsername.Text = "Nom d'utilisateur"; 
            textBoxUsername.ForeColor = Color.Gray;

            

            textBoxUsername.Enter += (sender, e) =>
            {
                if (textBoxUsername.Text == "Nom d'utilisateur")
                {
                    textBoxUsername.Text = "";                     
                    textBoxUsername.ForeColor = Color.Black;       
                }
            };

            textBoxUsername.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBoxUsername.Text))
                {
                    textBoxUsername.Text = "Nom d'utilisateur"; 
                    textBoxUsername.ForeColor = Color.Gray;
                }
            };
        }

        private void InitializePlaceholder1()
        {
            textBoxPassword.Text = "Mot de passe";
            textBoxPassword.ForeColor = Color.Gray;

            textBoxPassword.UseSystemPasswordChar = false;

            textBoxPassword.Enter += (sender, e) =>
            {
                if (textBoxPassword.Text == "Mot de passe")
                {
                    textBoxPassword.Text = "";
                    textBoxPassword.ForeColor = Color.Black;

                    textBoxPassword.UseSystemPasswordChar = true;
                }
            };

            textBoxPassword.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
                {
                    textBoxPassword.Text = "Mot de passe";
                    textBoxPassword.ForeColor = Color.Gray;

                    textBoxPassword.UseSystemPasswordChar = false;
                }
            };
        }





        private void quitterBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir quitter ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void connexionBtn_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;  
            string password = textBoxPassword.Text;  


            if (username == "admin" && password == "admin123")
            {

                Home newForm = new Home();
                newForm.Show();


                this.Hide();
            }
            else
            {

                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
