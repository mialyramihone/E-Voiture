using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CFFAMMA.TECHNIQUE
{
    public partial class retraitControl : UserControl
    {
        public retraitControl()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //ChargerIdProduitsDansComboBox();

          
        }

        private string connectionString = "Server=localhost;Database=mlr1;Uid=root;Pwd=;";


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void ajoutBtn_Click(object sender, EventArgs e)
        {
            if (refP.SelectedIndex == -1 || string.IsNullOrEmpty(typeproduit.Text) || string.IsNullOrEmpty(nomP.Text) || quantiteP.Value <= 0 || dateP.Value == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            if (quantiteP.Value <= 0)
            {
                MessageBox.Show("Veuillez entrer une quantité supérieure à 0.");
                return;
            }

            string insertQuery = "INSERT INTO retrait (ID_PRODUIT, TYPE_PRODUIT, NOM_PRODUIT, QUANTITE_SORTIE, DATE_SORTIE) " +
                                  "VALUES (@idProduit, @typeProduit, @nomProduit,  @quantite, @dateRetrait)";

            string idProduit = refP.SelectedItem.ToString();
            string typeProduit = typeproduit.Text;
            string nomProduit = nomP.Text;
            decimal quantite = quantiteP.Value;
            string dateRetrait = dateP.Value.ToString("yyyy-MM-dd");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idProduit", idProduit);
                        command.Parameters.AddWithValue("@typeProduit", typeProduit);
                        command.Parameters.AddWithValue("@nomProduit", nomProduit);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@dateRetrait", dateRetrait);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Retrait ajouté avec succès !");
                            AfficherRetraits();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout du retrait.");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur inconnue : {ex.Message}");
                }
            }
        }

        private void AfficherRetraits()
        {
            string selectQuery = "SELECT * FROM retrait";

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

        //this.Load += retrait_Load;

        private void retrait_Load(object sender, EventArgs e)
        {
            AfficherRetraits();
        }

        private void ResetFields()
        {
            refP.SelectedIndex = -1;
            nomP.Clear();
            typeproduit.Clear();
            quantiteP.Value = 0;
            dateP.Value = DateTime.Now;
        }

        private void afficherBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                refP.SelectedItem = selectedRow.Cells["ID_PRODUIT"].Value?.ToString();
                typeproduit.Text = selectedRow.Cells["TYPE_PRODUIT"].Value?.ToString();
                nomP.Text = selectedRow.Cells["NOM_PRODUIT"].Value?.ToString();
                quantiteP.Value = Convert.ToDecimal(selectedRow.Cells["QUANTITE_SORTIE"].Value ?? 0);
                dateP.Value = Convert.ToDateTime(selectedRow.Cells["DATE_SORTIE"].Value ?? DateTime.Now);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne dans le tableau.");
            }
        }

        private void majBtn_Click(object sender, EventArgs e)
        {
            string idProduit = refP.SelectedItem?.ToString();
            string typeProduit = typeproduit.Text;
            string nomProduit = nomP.Text;
            decimal quantite = quantiteP.Value;
            string dateRetrait = dateP.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(idProduit) || string.IsNullOrEmpty(typeProduit) || string.IsNullOrEmpty(nomProduit) || quantite <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement et saisir une quantité valide.");
                return;
            }

            string updateQuery = "UPDATE retrait SET TYPE_PRODUIT = @typeProduit, NOM_PRODUIT = @nomProduit, QUANTITE_SORTIE = @quantite, DATE_SORTIE = @dateRetrait WHERE ID_PRODUIT = @idProduit";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idProduit", idProduit);
                        command.Parameters.AddWithValue("@typeProduit", typeProduit);
                        command.Parameters.AddWithValue("@nomProduit", nomProduit);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@dateRetrait", dateRetrait);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Retrait mis à jour avec succès !");
                            AfficherRetraits();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Aucun produit trouvé avec cet ID.");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur MySQL lors de la mise à jour : {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue : {ex.Message}");
                }
            }
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un retrait à supprimer.");
                return;
            }

            string idProduit = dataGridView1.SelectedRows[0].Cells["ID_PRODUIT"].Value.ToString();

            if (string.IsNullOrEmpty(idProduit))
            {
                MessageBox.Show("Impossible de récupérer l'ID du produit. Veuillez réessayer.");
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce retrait ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation == DialogResult.No)
            {
                return;
            }

            string deleteQuery = "DELETE FROM retrait WHERE ID_PRODUIT= @idProduit";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idProduit", idProduit);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Retrait supprimé avec succès !");
                            AfficherRetraits();
                        }
                        else
                        {
                            MessageBox.Show("Aucun produit trouvé avec cet ID.");
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
