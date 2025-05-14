using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using static CFFAMMA.TECHNIQUE.homeControl;

namespace CFFAMMA.TECHNIQUE
{
    public partial class paiementControl : UserControl
    {
        private readonly string connectionString = "Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;";

        public paiementControl()
        {
            InitializeComponent();
            ConfigureDataGridView();
            InitializePlaceholder();


            PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche par ID, voiture ou client");
            Recherchetxt.TextChanged += new EventHandler(Recherchetxt_TextChanged);

            try
            {
                IdCommande.SelectedIndexChanged += IdCommande_SelectedIndexChanged;
                PrixNum.ValueChanged += PrixNum_ValueChanged;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors de la connexion des événements : {ex.Message}");
            }

            LoadData();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }

        private void InitializePlaceholder()
        {
            PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche");
        }

        private void LoadData()
        {
            ChargerIdCommandes();
            AfficherPaiements();
        }

        private void ChargerIdCommandes()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT IdCommande FROM commande";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        IdCommande.Items.Clear();
                        while (reader.Read())
                        {
                            string id = reader["IdCommande"].ToString();
                            IdCommande.Items.Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors du chargement des ID de commandes : {ex.Message}");
            }
        }

        private void AfficherDetailsCommande(string idCommande)
        {
            if (string.IsNullOrWhiteSpace(idCommande)) return;

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    string checkQuery = "SELECT COUNT(*) FROM commande WHERE IdCommande = @idCommande";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@idCommande", idCommande);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            VoitureTxt.Clear();
                            ClientTxt.Clear();
                            QuantiteTxt.Clear();
                            MontantTxt.Clear();
                            return;
                        }
                    }


                    string query = "SELECT voiture, client, quantite, prix FROM commande WHERE IdCommande = @idCommande";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", idCommande);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                string voiture = reader["voiture"].ToString();
                                string client = reader["client"].ToString();
                                string quantite = reader["quantite"].ToString();
                                decimal prixCommande = Convert.ToDecimal(reader["prix"]);
                                decimal prixLivraison = PrixNum.Value;
                                decimal montantTotal = prixCommande + prixLivraison;


                                VoitureTxt.Text = voiture;
                                ClientTxt.Text = client;
                                QuantiteTxt.Text = quantite;
                                MontantTxt.Text = montantTotal.ToString("N2");
                            }
                            else
                            {

                                VoitureTxt.Clear();
                                ClientTxt.Clear();
                                QuantiteTxt.Clear();
                                MontantTxt.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors de la récupération des détails : {ex.Message}");

                VoitureTxt.Clear();
                ClientTxt.Clear();
                QuantiteTxt.Clear();
                MontantTxt.Clear();
            }
        }

        private void AfficherPaiements()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM livraison";
                    using (var command = new MySqlCommand(query, connection))
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors du chargement des paiements : {ex.Message}");
            }
        }

        private void ResetFields()
        {
            IdCommande.SelectedIndex = -1;
            VoitureTxt.Clear();
            ClientTxt.Clear();
            QuantiteTxt.Clear();
            PrixNum.Value = 0;
            MontantTxt.Clear();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(IdCommande.Text) ||
                string.IsNullOrWhiteSpace(VoitureTxt.Text) ||
                string.IsNullOrWhiteSpace(ClientTxt.Text) ||
                string.IsNullOrWhiteSpace(QuantiteTxt.Text) ||
                string.IsNullOrWhiteSpace(MontantTxt.Text) ||
                MontantTxt.Text == "0.00")
            {
                ShowErrorMessage("Veuillez remplir tous les champs correctement");
                return false;
            }
            return true;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Événements
        private void IdCommande_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (IdCommande.SelectedItem != null)
                {
                    string selectedId = IdCommande.SelectedItem.ToString();
                    AfficherDetailsCommande(selectedId);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors de la sélection d'une commande : {ex.Message}");
            }
        }

        private void PrixNum_ValueChanged(object sender, EventArgs e)
        {

            if (IdCommande.SelectedItem != null)
            {
                AfficherDetailsCommande(IdCommande.SelectedItem.ToString());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];


                if (row.Cells["IdCommande"].Value != null)
                {

                    string idCommande = row.Cells["IdCommande"].Value.ToString();
                    int index = IdCommande.FindStringExact(idCommande);
                    if (index != -1)
                    {
                        IdCommande.SelectedIndex = index;
                    }
                    else
                    {

                        IdCommande.Items.Add(idCommande);
                        IdCommande.SelectedItem = idCommande;
                    }
                }

                VoitureTxt.Text = row.Cells["voiture"].Value?.ToString() ?? "";
                ClientTxt.Text = row.Cells["client"].Value?.ToString() ?? "";
                QuantiteTxt.Text = row.Cells["quantite"].Value?.ToString() ?? "";

                decimal prixLivraison = 0;
                if (row.Cells["prixlivraison"].Value != null &&
                    decimal.TryParse(row.Cells["prixlivraison"].Value.ToString(), out prixLivraison))
                {
                    PrixNum.Value = prixLivraison;
                }

                MontantTxt.Text = row.Cells["montant"].Value?.ToString() ?? "";
            }
        }

        private void ajoutBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            string query = @"INSERT INTO livraison(IdCommande, voiture, client, quantite, prixlivraison, montant, date) 
                          VALUES (@idCommande, @voiture, @client, @quantite, @prixlivraison, @montant, @date)";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", IdCommande.Text);
                        command.Parameters.AddWithValue("@voiture", VoitureTxt.Text);
                        command.Parameters.AddWithValue("@client", ClientTxt.Text);
                        command.Parameters.AddWithValue("@quantite", QuantiteTxt.Text);
                        command.Parameters.AddWithValue("@prixlivraison", PrixNum.Value);
                        command.Parameters.AddWithValue("@montant", decimal.Parse(MontantTxt.Text));
                        command.Parameters.AddWithValue("@date", DateTime.Now);

                        command.ExecuteNonQuery();
                        ShowSuccessMessage("Livraison ajoutée avec succès");
                        AfficherPaiements();
                        ResetFields();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors de l'ajout : {ex.Message}");
            }
        }
        private void afficherBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];


                if (row.Cells["IdCommande"].Value != null)
                {
                    string idCommande = row.Cells["IdCommande"].Value.ToString();
                    int index = IdCommande.FindStringExact(idCommande);
                    if (index != -1)
                    {
                        IdCommande.SelectedIndex = index;
                    }
                    else
                    {
                        IdCommande.Items.Add(idCommande);
                        IdCommande.SelectedItem = idCommande;
                    }
                }

                VoitureTxt.Text = row.Cells["voiture"].Value?.ToString() ?? "";
                ClientTxt.Text = row.Cells["client"].Value?.ToString() ?? "";
                QuantiteTxt.Text = row.Cells["quantite"].Value?.ToString() ?? "";

                if (decimal.TryParse(row.Cells["prixlivraison"].Value?.ToString(), out decimal prixLivraison))
                {
                    PrixNum.Value = prixLivraison;
                }

                MontantTxt.Text = row.Cells["montant"].Value?.ToString() ?? "";
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un paiement dans le tableau.",
                               "Aucune sélection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
            }
        }

        private void majBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une livraison à modifier.",
                               "Aucune sélection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateFields()) return;


            int IdCommande = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IdCommande"].Value);

            string updateQuery = @"UPDATE livraison SET 
                                 voiture = @voiture, 
                                 client = @client, 
                                 quantite = @quantite, 
                                 prixlivraison = @prixlivraison,
                                 montant = @montant 
                                 WHERE IdCommande = @idCommande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", IdCommande);
                        command.Parameters.AddWithValue("@voiture", VoitureTxt.Text);
                        command.Parameters.AddWithValue("@client", ClientTxt.Text);
                        command.Parameters.AddWithValue("@quantite", QuantiteTxt.Text);
                        command.Parameters.AddWithValue("@prixlivraison", PrixNum.Value);
                        command.Parameters.AddWithValue("@montant", decimal.Parse(MontantTxt.Text));

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            ShowSuccessMessage("Livraison mise à jour avec succès !");
                            AfficherPaiements();
                            ResetFields();
                        }
                        else
                        {
                            ShowErrorMessage("Aucune livraison trouvée avec cet ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Erreur lors de la mise à jour : {ex.Message}");
                }
            }
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une livraison à supprimer.",
                               "Aucune sélection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            int IdCommande = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IdCommande"].Value);

            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cette livraison ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation == DialogResult.No) return;

            string deleteQuery = "DELETE FROM livraison WHERE IdCommande = @idCommande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", IdCommande);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ShowSuccessMessage("Livraison supprimée avec succès !");
                            AfficherPaiements();
                            ResetFields();
                        }
                        else
                        {
                            ShowErrorMessage("Aucune livraison trouvée avec cet ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Erreur lors de la suppression : {ex.Message}");
                }
            }
        }
        

        private void Recherchetxt_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Recherchetxt.Text) ||
                Recherchetxt.Text == "Recherche")
            {
                AfficherPaiements();
                return;
            }

            try
            {
                string searchValue = Recherchetxt.Text.Trim();

                string query = @"SELECT * FROM livraison 
                      WHERE IdCommande LIKE @search 
                      OR voiture LIKE @search 
                      OR client LIKE @search ";

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@search", $"%{searchValue}%");

                        DataTable dt = new DataTable();
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Erreur lors de la recherche : {ex.Message}");
            }
        }
    }
}