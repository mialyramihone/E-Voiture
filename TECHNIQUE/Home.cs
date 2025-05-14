using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFFAMMA.TECHNIQUE
{


    public partial class Home : Form
    {

        private homeControl homeControl;
        private newproduit produitControl;
        private paiementControl paiementControl;
        private commandeControl commandeControl;
        private clientControl clientControl;

        

        public Home()
        {
            InitializeComponent();



            homeControl = new homeControl();
            produitControl = new newproduit();
            paiementControl = new paiementControl();
            commandeControl = new commandeControl();
            clientControl = new clientControl();



            ApplyUnderline(HomeBtn);
            HomeBtn.ForeColor = Color.FromArgb(0, 69, 91);
            HomeBtn.BackColor = Color.FromArgb(163, 227, 255);
            HomeBtn.Font = new Font(HomeBtn.Font, FontStyle.Bold); 


            LoadControl(homeControl);
        }

        private void LoadControl(UserControl control)
        {

            panelMain.Controls.Clear();

            control.Dock = DockStyle.Fill;
            panelMain.Controls.Add(control);


        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void agrandirBtn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void reduireBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void ApplyUnderline(Button btn)
        {

            btn.ForeColor = Color.FromArgb(0, 69, 91);
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.BackColor = Color.FromArgb(163, 227, 255);

            if (btn.Image != null)
            {
                btn.Image = RecolorImage((Bitmap)btn.Image, Color.FromArgb(0, 69, 91)); 
            }

            
        }

        private void RemoveUnderline(Button btn)
        {
            if (btn.Tag is Panel underline)
            {
                this.Controls.Remove(underline);
                btn.Tag = null;
            }

            btn.ForeColor = Color.Black;
            btn.Font = new Font(btn.Font, FontStyle.Regular);
            btn.BackColor = Color.Transparent;


            if (btn.Image != null)
            {
                btn.Image = RecolorImage((Bitmap)btn.Image, Color.Black);
            }

        }


        private Bitmap RecolorImage(Bitmap originalImage, Color newColor)
        {

            Bitmap coloredImage = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g = Graphics.FromImage(coloredImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
            new float[] {0, 0, 0, 0, 0}, // Red
            new float[] {0, 0, 0, 0, 0}, // Green
            new float[] {0, 0, 0, 0, 0}, // Blue
            new float[] {0, 0, 0, 1, 0}, // Alpha         
            new float[] {newColor.R / 255f, newColor.G / 255f, newColor.B / 255f, 0, 1}
                });

                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                        0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attributes);
                }
            }

            return coloredImage;
        }




        private void HomeBtn_Click(object sender, EventArgs e)
        {
            LoadControl(homeControl);
            ApplyUnderline(HomeBtn);
            RemoveUnderline(produitBtn);
            //RemoveUnderline(notifBtn);
            RemoveUnderline(entretienBtn);
            //RemoveUnderline(PaiementBtn);
            RemoveUnderline(CommandeBtn);
            RemoveUnderline(clientBtn);
            //RemoveUnderline(retraitBtn);
            //RemoveUnderline(stockageBtn);
            //RemoveUnderline(ApprovisionnementBtn);
            RemoveUnderline(parametreBtn);
            //RemoveUnderline(productionBtn);
        }

        private void produitBtn_Click(object sender, EventArgs e)
        {
            LoadControl(produitControl);
            ApplyUnderline(produitBtn);
            //RemoveUnderline(productionBtn);
            RemoveUnderline(HomeBtn);
            //RemoveUnderline(notifBtn);
            RemoveUnderline(entretienBtn);
            //RemoveUnderline(PaiementBtn);
            RemoveUnderline(CommandeBtn);
            RemoveUnderline(clientBtn);
            //RemoveUnderline(retraitBtn);
            //RemoveUnderline(stockageBtn);
            //RemoveUnderline(ApprovisionnementBtn);
            RemoveUnderline(parametreBtn);
        }

      
        private void entretienBtn_Click(object sender, EventArgs e)
        {
            LoadControl(paiementControl);
            ApplyUnderline(entretienBtn);
            RemoveUnderline(HomeBtn);
            //RemoveUnderline(productionBtn);
            RemoveUnderline(produitBtn);
            //RemoveUnderline(notifBtn);
            //RemoveUnderline(PaiementBtn);
            RemoveUnderline(CommandeBtn);
            RemoveUnderline(clientBtn);
            //RemoveUnderline(retraitBtn);
            //RemoveUnderline(stockageBtn);
            //RemoveUnderline(ApprovisionnementBtn);
            RemoveUnderline(parametreBtn);
        }


        //private void FactureBtn_Click(object sender, EventArgs e)
        //{
        //    LoadControl(factureControl);
        //    ApplyUnderline(FactureBtn);
        //    RemoveUnderline(HomeBtn);
        //    RemoveUnderline(produitBtn);
        //    RemoveUnderline(productionBtn);
        //    RemoveUnderline(technicienBtn);
        //    RemoveUnderline(notifBtn);
        //    RemoveUnderline(entretienBtn);
        //    RemoveUnderline(PaiementBtn);
        //    RemoveUnderline(CommandeBtn);
        //    RemoveUnderline(clientBtn);
        //    RemoveUnderline(retraitBtn);
        //    RemoveUnderline(stockBtn);
        //    RemoveUnderline(ApprovisionnementBtn);
        //    RemoveUnderline(parametreBtn);
        //}

        private void CommandeBtn_Click(object sender, EventArgs e)
        {
            LoadControl(commandeControl);
            ApplyUnderline(CommandeBtn);
            RemoveUnderline(HomeBtn);
            RemoveUnderline(produitBtn);
            //RemoveUnderline(productionBtn);
            //RemoveUnderline(notifBtn);
            RemoveUnderline(entretienBtn);
            //RemoveUnderline(PaiementBtn);
            RemoveUnderline(clientBtn);
            //RemoveUnderline(retraitBtn);
            //RemoveUnderline(stockageBtn);
            //RemoveUnderline(ApprovisionnementBtn);
            RemoveUnderline(parametreBtn);
        }

        private void clientBtn_Click(object sender, EventArgs e)
        {
            LoadControl(clientControl);
            ApplyUnderline(clientBtn);
            RemoveUnderline(HomeBtn);
            RemoveUnderline(produitBtn);
            //RemoveUnderline(productionBtn);
            //RemoveUnderline(notifBtn);
            RemoveUnderline(entretienBtn);
            //RemoveUnderline(PaiementBtn);
            RemoveUnderline(CommandeBtn);
            //RemoveUnderline(retraitBtn);
            //RemoveUnderline(stockageBtn);
            //RemoveUnderline(ApprovisionnementBtn);
            RemoveUnderline(parametreBtn);
        }


        private void parametreBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Voulez-vous vraiment vous déconnecter ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Connexion newForm = new Connexion();
                newForm.Show();

                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Hide();
                }
            }
        }




        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
                      
        }

    }
}
