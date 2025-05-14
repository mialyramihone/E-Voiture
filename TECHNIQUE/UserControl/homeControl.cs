using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Data.SQLite;

namespace CFFAMMA.TECHNIQUE
{
    public partial class homeControl : UserControl
    {


        private readonly string connectionString = "Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;";

        public homeControl()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;



        PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche par ID, voiture ou client");
            Recherchetxt.TextChanged += new EventHandler(Recherchetxt_TextChanged);

            AfficherNombreVoitures();
            AfficherNombreCommandes();
            AfficherNombreLivraisons();
            AfficherRevenusTotaux();
            ChargerCommandesAvecLivraison();

        }

        private void Recherchetxt_TextChanged(object sender, EventArgs e)
        {
            string recherche = Recherchetxt.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(recherche))
            {
                AfficherCommandesAvecLivraison();
                return;
            }

            string selectQuery = @"
        SELECT 
            c.IdCommande, 
            c.voiture, 
            c.client, 
            c.quantite, 
            COALESCE(l.prixlivraison, '-') AS prixlivraison,
            COALESCE(l.montant, c.prix) AS montant,
            c.date AS date_commande,
            l.date AS date_livraison
        FROM commande c
        LEFT JOIN livraison l ON c.IdCommande = l.IdCommande
        WHERE 
            c.IdCommande LIKE @recherche 
            OR LOWER(c.voiture) LIKE @recherche 
            OR LOWER(c.client) LIKE @recherche";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@recherche", "%" + recherche + "%");

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la recherche: {ex.Message}",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void AfficherCommandesAvecLivraison()
        {
            string selectQuery = @"
        SELECT 
            c.IdCommande, 
            c.voiture, 
            c.client, 
            c.quantite, 
            COALESCE(l.prixlivraison, '-') AS prixlivraison,
            COALESCE(l.montant, c.prix) AS montant,
            c.date AS date_commande,
            l.date AS date_livraison
        FROM commande c
        LEFT JOIN livraison l ON c.IdCommande = l.IdCommande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public static class PlaceholderHelper
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

            private const int EM_SETCUEBANNER = 0x1501;

            public static void SetPlaceholder(TextBox textBox, string placeholder)
            {
                SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, placeholder);
            }
        }

        private void AfficherNombreVoitures()
        {
            using (MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;"))
            {
                try
                {
                    connexion.Open();
                    string requete = "SELECT COUNT(*) FROM voiture";
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    int nombreVoitures = Convert.ToInt32(cmd.ExecuteScalar());
                    VoitureTxt.Text = nombreVoitures.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération du nombre de voitures : " + ex.Message);
                }
            }
        }

        private void AfficherNombreCommandes()
        {
            using (MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;"))
            {
                try
                {
                    connexion.Open();
                    string requete = "SELECT COUNT(*) FROM commande";
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    int nombreCommandes = Convert.ToInt32(cmd.ExecuteScalar());
                    ComandeTxt.Text = nombreCommandes.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération du nombre de commandes : " + ex.Message);
                }
            }
        }

        private void AfficherNombreLivraisons()
        {
            using (MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;"))
            {
                try
                {
                    connexion.Open();
                    string requete = "SELECT COUNT(*) FROM livraison";
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    int nombreLivraisons = Convert.ToInt32(cmd.ExecuteScalar());
                    LivraisonTxt.Text = nombreLivraisons.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération du nombre de livraisons : " + ex.Message);
                }
            }
        }


        private void AfficherRevenusTotaux()
        {
            using (MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;"))
            {
                try
                {
                    connexion.Open();
                    string requete = "SELECT SUM(prix) FROM commande";
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    object resultat = cmd.ExecuteScalar();
                    decimal total = resultat != DBNull.Value ? Convert.ToDecimal(resultat) : 0;
                    PrixTxt.Text = total.ToString("N2") + " Ar";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors du calcul des revenus : " + ex.Message);
                }
            }
        }

        private void ChargerCommandesAvecLivraison()
        {
            using (MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;"))
            {
                try
                {
                    connexion.Open();
                    string requete = @"
                SELECT 
                    c.IdCommande AS 'N° Commande',
                    c.voiture AS 'Voiture',
                    c.client AS 'Client',
                    c.quantite AS 'Quantité',
                    c.prix AS 'Prix Unitaire',
                    c.date AS 'Date Commande',
                    l.prixlivraison AS 'Prix Livraison',
                    l.montant AS 'Montant Total',
                    l.date AS 'Date Livraison'
                FROM 
                    commande c
                LEFT JOIN 
                    livraison l ON c.IdCommande = l.IdCommande";

                    MySqlDataAdapter adaptateur = new MySqlDataAdapter(requete, connexion);
                    DataTable table = new DataTable();
                    adaptateur.Fill(table);
                    dataGridView1.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors du chargement des commandes avec livraisons : " + ex.Message);
                }
            }
        }







    }
}
