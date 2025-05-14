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
using System.Runtime.InteropServices;
using static CFFAMMA.TECHNIQUE.homeControl;

namespace CFFAMMA.TECHNIQUE
{
    public partial class newproduit : UserControl
    {
        public newproduit()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;




            PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche par ID, marque ou modele");
            Recherchetxt.TextChanged += new EventHandler(Recherchetxt_TextChanged);


        }

        private void Recherchetxt_TextChanged(object sender, EventArgs e)
        {
            string recherche = Recherchetxt.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(recherche))
            {
                AfficherProduits();
                return;
            }

            string selectQuery = @"SELECT IdVoiture, marque, modele, couleur, prix, annee FROM voiture 
                                WHERE IdVoiture LIKE @recherche 
                                OR LOWER(marque) LIKE @recherche 
                                OR LOWER(modele) LIKE @recherche";

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


        private string connectionString = "Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;";

        private void newproduit_Load(object sender, EventArgs e)
        {
            AfficherProduits();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void ajoutBtn_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(IdVoiture.Text) ||
                string.IsNullOrEmpty(MarqueTxt.Text) ||
                string.IsNullOrEmpty(ModeleTxt.Text) ||
                couleurBox.SelectedIndex == -1 ||
                PrixNum.Value <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement.");
                return;
            }

            string idVoiture = IdVoiture.Text;
            string marque = MarqueTxt.Text;
            string modele = ModeleTxt.Text;
            string couleur = couleurBox.SelectedItem.ToString();
            decimal prix = PrixNum.Value;
            string annee = DateVoiture.Value.ToString("yyyy-MM-dd");

            if (prix <= 0)
            {
                MessageBox.Show("Veuillez entrer un prix supérieur à 0.");
                return;
            }

            string insertQuery = "INSERT INTO voiture(IdVoiture, marque, modele, couleur, prix, annee) " +
                                 "VALUES (@idVoiture, @marque, @modele, @couleur, @prix, @annee)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idVoiture", idVoiture);
                        command.Parameters.AddWithValue("@marque", marque);
                        command.Parameters.AddWithValue("@modele", modele);
                        command.Parameters.AddWithValue("@couleur", couleur);
                        command.Parameters.AddWithValue("@prix", prix);
                        command.Parameters.AddWithValue("@annee", annee);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Voiture ajoutée avec succès !");
                            AfficherProduits();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout de la voiture.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de connexion ou d'insertion : {ex.Message}");
                }
            }
        }

        private void AfficherProduits()
        {
            string selectQuery = "SELECT * FROM voiture"; 

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de récupération des données : {ex.Message}");
                }
            }
        }


        private void refP_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }


        private void majBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IdVoiture.Text) ||
                string.IsNullOrEmpty(MarqueTxt.Text) ||
                string.IsNullOrEmpty(ModeleTxt.Text) ||
                couleurBox.SelectedIndex == -1 ||
                PrixNum.Value <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement.");
                return;
            }

            string idVoiture = IdVoiture.Text;
            string marque = MarqueTxt.Text;
            string modele = ModeleTxt.Text;
            string couleur = couleurBox.SelectedItem.ToString();
            decimal prix = PrixNum.Value;
            string annee = DateVoiture.Value.ToString("yyyy-MM-dd");

            string updateQuery = "UPDATE voiture SET marque = @marque, modele = @modele, " +
                                 "couleur = @couleur, prix = @prix, annee = @annee " +
                                 "WHERE IdVoiture = @idVoiture";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idVoiture", idVoiture);
                        command.Parameters.AddWithValue("@marque", marque);
                        command.Parameters.AddWithValue("@modele", modele);
                        command.Parameters.AddWithValue("@couleur", couleur);
                        command.Parameters.AddWithValue("@prix", prix);
                        command.Parameters.AddWithValue("@annee", annee);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Voiture mise à jour avec succès !");
                            AfficherProduits();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Aucune voiture trouvée avec cet ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}");
                }
            }
        }


        private void ResetFields()
        {
            IdVoiture.Clear();
            MarqueTxt.Clear();
            ModeleTxt.Clear();
            couleurBox.SelectedIndex = -1;
            PrixNum.Value = 0;
            DateVoiture.Value = DateTime.Now;
        }

        private void afficherBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                IdVoiture.Text = selectedRow.Cells["IdVoiture"].Value?.ToString();
                MarqueTxt.Text = selectedRow.Cells["marque"].Value?.ToString();
                ModeleTxt.Text = selectedRow.Cells["modele"].Value?.ToString();
                couleurBox.SelectedItem = selectedRow.Cells["couleur"].Value?.ToString();
                PrixNum.Value = Convert.ToDecimal(selectedRow.Cells["prix"].Value ?? 0);
                DateVoiture.Value = Convert.ToDateTime(selectedRow.Cells["annee"].Value ?? DateTime.Now);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne dans le tableau.");
            }
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une voiture à supprimer.");
                return;
            }

            string idVoiture = dataGridView1.SelectedRows[0].Cells["IdVoiture"].Value.ToString();

            if (string.IsNullOrEmpty(idVoiture))
            {
                MessageBox.Show("Impossible de récupérer l'ID de la voiture. Veuillez réessayer.");
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cette voiture ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation == DialogResult.No)
            {
                return;
            }

            string deleteQuery = "DELETE FROM voiture WHERE IdVoiture = @idVoiture";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idVoiture", idVoiture);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Voiture supprimée avec succès !");
                            AfficherProduits();
                        }
                        else
                        {
                            MessageBox.Show("Aucune voiture trouvée avec cet ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}");
                }
            }
        }

    }
}
