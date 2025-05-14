using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFFAMMA.TECHNIQUE
{
    public partial class productionControl : UserControl
    {
        public productionControl()
        {
            InitializeComponent();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ChargerIdProduitsDansComboBox();

            refP.SelectedIndexChanged += refP_SelectedIndexChanged;


        }

        private int _initialProductId;

        private string connectionString = "Server=localhost;Database=mlr1;Uid=root;Pwd=;";

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ResetFields()
        {
            refproduction.Clear();
            refP.SelectedIndex = -1;
            nomP.Clear();
            quantiteP.Value = 0;
            dateP.Value = DateTime.Now;
        }

        //this.Load += new System.EventHandler(this.approvisionnement_Load);

        private void production_Load(object sender, EventArgs e)
        {
            AfficherProduits();
        }

        private void AfficherProduits()
        {
            string selectQuery = "SELECT * FROM production";

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

        private void ajoutBtn_Click(object sender, EventArgs e)
        {

            if (
                string.IsNullOrEmpty(refproduction.Text) ||
                refP.SelectedIndex == -1 ||
                string.IsNullOrEmpty(nomP.Text) ||
                quantiteP.Value <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement.");
                return;
            }

            string idProduction = refproduction.Text;
            string idMateriel = refP.SelectedItem.ToString();
            string nomMateriel = nomP.Text;
            decimal quantite = quantiteP.Value;

            if (quantite <= 0)
            {
                MessageBox.Show("Veuillez entrer une quantité supérieur à 0.");
                return;
            }

            string dateProduction = dateP.Value.ToString("yyyy-MM-dd");


            string insertQuery = "INSERT INTO production (ID_PRODUCTION, ID_MATERIEL, NOM_MATERIEL, QUANTITE, DATE_PRODUCTION) " +
                                 "VALUES (@idProduction, @idMateriel, @nomMateriel, @quantite, @dateProduction)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idProduction", idProduction);
                        command.Parameters.AddWithValue("@idMateriel", idMateriel);
                        command.Parameters.AddWithValue("@nomMateriel", nomMateriel);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@dateProduction", dateProduction);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Production ajouté avec succès !");
                            AfficherProduits();
                            ResetFields();
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout du production.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de connexion ou d'insertion : {ex.Message}");
                }
            }
        }


        private void afficherBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];


                refproduction.Text = selectedRow.Cells["ID_PRODUCTION"].Value?.ToString();
                refP.SelectedItem = selectedRow.Cells["ID_MATERIEL"].Value?.ToString();
                nomP.Text = selectedRow.Cells["NOM_MATERIEL"].Value?.ToString();
                quantiteP.Value = Convert.ToDecimal(selectedRow.Cells["QUANTITE"].Value ?? 0);
                dateP.Value = Convert.ToDateTime(selectedRow.Cells["DATE_PRODUCTION"].Value ?? DateTime.Now);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne dans le tableau.");
            }
        }

        private void majBtn_Click(object sender, EventArgs e)
        {
            string idProduction = refproduction.Text;
            string idMateriel = refP.SelectedItem?.ToString();
            string nomMateriel = nomP.Text;
            decimal quantite = quantiteP.Value;
            string dateProduction = dateP.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(idProduction) || string.IsNullOrEmpty(idMateriel) || string.IsNullOrEmpty(nomMateriel) || quantite <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs correctement et saisir une quantité valide.");
                return;
            }

            string updateQuery = "UPDATE production SET ID_MATERIEL = @idMateriel, NOM_MATERIEL = @nomMateriel, " +
                                        "QUANTITE = @quantite, DATE_PRODUCTION = @dateProduction WHERE ID_PRODUCTION = @idProduction";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idProduction", idProduction);
                        command.Parameters.AddWithValue("@idMateriel", idMateriel);
                        command.Parameters.AddWithValue("@nomMateriel", nomMateriel);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@dateProduction", dateProduction);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Production mise à jour avec succès !");
                            AfficherProduits();
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


        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une production à supprimer.");
                return;
            }


            string idProduction = dataGridView1.SelectedRows[0].Cells["ID_PRODUCTION"].Value.ToString();

            if (string.IsNullOrEmpty(idProduction))
            {
                MessageBox.Show("Impossible de récupérer l'ID du production. Veuillez réessayer.");
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

            string deleteQuery = "DELETE FROM production WHERE ID_PRODUCTION= @idProduction";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idProduction", idProduction);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Produit supprimé avec succès !");
                            AfficherProduits();
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

        private void refP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (refP.SelectedIndex != -1)
            {
                try
                {
                    int selectedId = int.Parse(refP.SelectedItem.ToString()); 
                    AfficherNomProduit(selectedId); 
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Erreur de format de l'ID produit : " + ex.Message);
                    nomP.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue : " + ex.Message);
                    nomP.Text = "";
                }
            }
            else
            {
                nomP.Text = ""; 
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {


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

        private void nomP_TextChanged(object sender, EventArgs e)
        {
            
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
       
    }
}
