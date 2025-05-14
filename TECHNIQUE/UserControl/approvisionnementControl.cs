using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFFAMMA.TECHNIQUE
{
    public partial class approvisionnementControl : UserControl
    {
        public approvisionnementControl()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ChargerIdProduitsDansComboBox();

        }

        private string connectionString = "Server=localhost;Database=mlr1;Uid=root;Pwd=;";


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
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
                MessageBox.Show("Veuillez entrer une quantité supérieur à 0.");
                return;
            }

            string insertQuery = "INSERT INTO approvisionnement (ID_PRODUIT, TYPE_PRODUIT, NOM_PRODUIT, QUANTITE_ENTREE, DATE_ENTREE) " +
                                  "VALUES (@idProduit, @typeProduit, @nomProduit, @quantite, @dateAppro)";

            string idProduit = refP.SelectedItem.ToString();
            string typeProduit = typeproduit.Text;
            string nomProduit = nomP.Text;
            decimal quantite = quantiteP.Value;
            string dateAppro = dateP.Value.ToString("yyyy-MM-dd");

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
                        command.Parameters.AddWithValue("@dateAppro", dateAppro);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Approvisionnement ajouté avec succès !");
                            AfficherApprovisionnements();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout de l'approvisionnement.");
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

        
        private void AfficherApprovisionnements()
        {
            string selectQuery = "SELECT * FROM approvisionnement";

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

        private void approvisionnement_Load(object sender, EventArgs e)
        {
            AfficherApprovisionnements();
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
                quantiteP.Value = Convert.ToDecimal(selectedRow.Cells["QUANTITE_ENTREE"].Value ?? 0);
                dateP.Value = Convert.ToDateTime(selectedRow.Cells["DATE_ENTREE"].Value ?? DateTime.Now);
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
            string dateAppro = dateP.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(idProduit) || string.IsNullOrEmpty(typeProduit) || string.IsNullOrEmpty(nomProduit) || quantite <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement et saisir une quantité valide.");
                return;
            }

            string updateQuery = "UPDATE approvisionnement SET TYPE_PRODUIT = @typeProduit, NOM_PRODUIT = @nomProduit, QUANTITE_ENTREE = @quantite, DATE_ENTREE = @dateAppro WHERE ID_PRODUIT = @idProduit";

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
                        command.Parameters.AddWithValue("@dateAppro", dateAppro);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Approvisionnement mise à jour avec succès !");
                            AfficherApprovisionnements();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Aucune production trouvée avec cet ID.");
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

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (refP.SelectedIndex != -1)
            {
                try
                {
                    int selectedId = int.Parse(refP.SelectedItem.ToString()); 
                    AfficherNomProduit(selectedId);
                    AfficherTypeProduit(selectedId);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Erreur de format de l'ID produit : " + ex.Message);
                    typeproduit.Text = ""; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message);
                    typeproduit.Text = "";
                }
            }
            else
            {
                typeproduit.Text = "";
            }
        }


        private void AfficherNomProduit(int idMateriel)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT NOM_PRODUIT FROM produit WHERE ID_PRODUIT = @id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", idMateriel);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {


                            if (reader.Read())
                            {
                                nomP.Text = reader["NOM_PRODUIT"].ToString();
                            }

                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur MySQL : " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }

        }
        private void AfficherTypeProduit(int idMateriel)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT TYPE_PRODUIT FROM produit WHERE ID_PRODUIT = @id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", idMateriel);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {


                            if (reader.Read())
                            {
                                typeproduit.Text = reader["TYPE_PRODUIT"].ToString();
                            }

                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erreur MySQL : " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }

        }

        private void ChargerIdProduitsDansComboBox()
        {
            string query = "SELECT ID_PRODUIT FROM produit";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            refP.Items.Clear();

                            while (reader.Read())
                            {
                                refP.Items.Add(reader["ID_PRODUIT"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des IDs : {ex.Message}");
                }
            }
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une production à supprimer.");
                return;
            }


            string idProduit = dataGridView1.SelectedRows[0].Cells["ID_PRODUIT"].Value.ToString();

            if (string.IsNullOrEmpty(idProduit))
            {
                MessageBox.Show("Impossible de récupérer l'ID du produit. Veuillez réessayer.");
                return;
            }


            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce produit ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation == DialogResult.No)
            {
                return;
            }

            string deleteQuery = "DELETE FROM approvisionnement WHERE ID_PRODUIT= @idProduit";

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
                            MessageBox.Show("Approvisionnement supprimé avec succès !");
                            AfficherApprovisionnements();
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
