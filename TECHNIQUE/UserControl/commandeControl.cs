using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static CFFAMMA.TECHNIQUE.homeControl;

namespace CFFAMMA.TECHNIQUE
{
    public partial class commandeControl : UserControl
    {
        private readonly paiementControl paiementControl;
        private readonly string connectionString = "Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;";

        public commandeControl()
        {
            InitializeComponent();
            paiementControl = new paiementControl();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche");

            ChargerVoitures();
            ChargerClients();
            GenererRefCommande();
            AfficherCommandes();

            CalculerPrixTotal();


            PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche par ID, voiture ou client");
            Recherchetxt.TextChanged += new EventHandler(Recherchetxt_TextChanged);
        

            PrixTxt.ReadOnly = true;
            PrixTxt.BackColor = Color.White;
            PrixTxt.Font = new Font(PrixTxt.Font, FontStyle.Bold);

            VoitureBox.SelectedIndexChanged += VoitureBox_SelectedIndexChanged;
            QuantiteNum.ValueChanged += QuantiteNum_ValueChanged;

        }

        

        private decimal GetPrixUnitaire(string voitureInfo)
        {
            string[] parts = voitureInfo.Split(new[] { " - " }, StringSplitOptions.None);
            if (parts.Length < 1) return 0;

            string marqueModele = parts[0];
            string[] marqueModeleParts = marqueModele.Split(' ');
            if (marqueModeleParts.Length < 2) return 0;

            string marque = marqueModeleParts[0];
            string modele = marqueModeleParts[1];

            string query = "SELECT prix FROM voiture WHERE marque = @marque AND modele = @modele LIMIT 1";

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@marque", marque);
                    command.Parameters.AddWithValue("@modele", modele);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        private void commandeControl_Load(object sender, EventArgs e)
        {
            GenererRefCommande();
        }

        private void GenererRefCommande()
        {
            int nextCommandeNumber = GetNextCommandeNumber();
            IdCommande.Text = nextCommandeNumber.ToString();
        }

        private int GetNextCommandeNumber()
        {
            int lastNumber = 0;
            string selectQuery = "SELECT MAX(CAST(IdCommande AS UNSIGNED)) FROM commande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            lastNumber = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur: {ex.Message}");
                }
            }
            return lastNumber + 1;
        }

        private void ajoutBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IdCommande.Text) ||
                VoitureBox.SelectedIndex == -1 ||
                ClientBox.SelectedIndex == -1 ||
                QuantiteNum.Value <= 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.",
                              "Champs manquants",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            string voitureInfo = VoitureBox.Text;
            string clientInfo = ClientBox.Text;
            int quantite = (int)QuantiteNum.Value;
            decimal prixTotal = GetPrixTotal(voitureInfo, quantite);

            if (prixTotal <= 0)
            {
                MessageBox.Show("Prix invalide. Vérifiez la sélection de la voiture.",
                              "Erreur",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            string idCommande = IdCommande.Text.Trim();
            string dateCommande = DateTime.Now.ToString("yyyy-MM-dd");

            string insertQuery = @"INSERT INTO commande(IdCommande, voiture, client, quantite, prix, date) 
                              VALUES (@idCommande, @voiture, @client, @quantite, @prix, @date)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (CommandeExists(idCommande))
                    {
                        MessageBox.Show("Cette commande existe déjà.",
                                      "Erreur",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", idCommande);
                        command.Parameters.AddWithValue("@voiture", voitureInfo);
                        command.Parameters.AddWithValue("@client", clientInfo);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@prix", prixTotal);
                        command.Parameters.AddWithValue("@date", dateCommande);

                        if (command.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Commande ajoutée avec succès !",
                                          "Succès",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                            AfficherCommandes();
                            ResetFields();
                            GenererRefCommande();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur: {ex.Message}",
                                  "Erreur",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private decimal GetPrixTotal(string voitureInfo, int quantite)
        {
            string[] parts = voitureInfo.Split(new[] { " - " }, StringSplitOptions.None);
            if (parts.Length < 1) return 0;

            string marqueModele = parts[0];
            string[] marqueModeleParts = marqueModele.Split(' ');
            if (marqueModeleParts.Length < 2) return 0;

            string marque = marqueModeleParts[0];
            string modele = marqueModeleParts[1];

            string query = "SELECT prix FROM voiture WHERE marque = @marque AND modele = @modele LIMIT 1";

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@marque", marque);
                    command.Parameters.AddWithValue("@modele", modele);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) * quantite : 0;
                }
                catch
                {
                    return 0;
                }
            }
        }

        private bool CommandeExists(string idCommande)
        {
            string query = "SELECT COUNT(*) FROM commande WHERE IdCommande = @id";
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@id", idCommande);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de vérification: {ex.Message}",
                                  "Erreur",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void ChargerVoitures()
        {
            string query = "SELECT CONCAT(marque, ' ', modele, ' - ', couleur) AS info FROM voiture";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    VoitureBox.DataSource = table;
                    VoitureBox.DisplayMember = "info";

                    if (table.Rows.Count > 0)
                    {
                        VoitureBox.SelectedIndex = 0;

                        CalculerPrixTotal();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur voitures: {ex.Message}");
                }
            }
        }

        private void ChargerClients()
        {
            string query = "SELECT CONCAT(nom, ' ', prenom) AS nom_complet FROM client";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    ClientBox.DataSource = table;
                    ClientBox.DisplayMember = "nom_complet";

                    if (table.Rows.Count > 0)
                        ClientBox.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur clients: {ex.Message}");
                }
            }
        }

        private void afficherBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                IdCommande.Text = row.Cells["IdCommande"].Value?.ToString();
                VoitureBox.Text = row.Cells["voiture"].Value?.ToString();
                ClientBox.Text = row.Cells["client"].Value?.ToString();
                QuantiteNum.Value = Convert.ToInt32(row.Cells["quantite"].Value ?? 0);

                if (decimal.TryParse(row.Cells["prix"].Value?.ToString(), out decimal prix))
                {
                    PrixTxt.Text = prix.ToString("F2") + " Ar";
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande dans le tableau.",
                               "Aucune sélection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
            }
        }

        private void majBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande à modifier.",
                               "Aucune sélection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            string idCommande = IdCommande.Text.Trim();
            string voitureInfo = VoitureBox.Text;
            string clientInfo = ClientBox.Text;
            int quantite = (int)QuantiteNum.Value;
            decimal prixTotal = GetPrixTotal(voitureInfo, quantite);

            if (prixTotal <= 0)
            {
                MessageBox.Show("Prix invalide. Vérifiez la sélection de la voiture.",
                              "Erreur",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            string updateQuery = @"UPDATE commande SET 
                                 voiture = @voiture, 
                                 client = @client, 
                                 quantite = @quantite, 
                                 prix = @prix 
                                 WHERE IdCommande = @idCommande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", idCommande);
                        command.Parameters.AddWithValue("@voiture", voitureInfo);
                        command.Parameters.AddWithValue("@client", clientInfo);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@prix", prixTotal);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Commande mise à jour avec succès !",
                                          "Succès",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                            AfficherCommandes();
                            ResetFields();
                            GenererRefCommande();
                        }
                        else
                        {
                            MessageBox.Show("Aucune commande trouvée avec cet ID.",
                                          "Erreur",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}",
                                  "Erreur",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.",
                               "Aucune sélection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            string idCommande = dataGridView1.SelectedRows[0].Cells["IdCommande"].Value?.ToString();

            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer cette commande ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation == DialogResult.No) return;

            string deleteQuery = "DELETE FROM commande WHERE IdCommande = @idCommande";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idCommande", idCommande);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Commande supprimée avec succès !",
                                          "Succès",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                            AfficherCommandes();
                            ResetFields();
                            GenererRefCommande();
                        }
                        else
                        {
                            MessageBox.Show("Aucune commande trouvée avec cet ID.",
                                         "Erreur",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}",
                                  "Erreur",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void AfficherCommandes()
        {
            string selectQuery = "SELECT IdCommande, voiture, client, quantite, prix, date FROM commande";

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
                    MessageBox.Show($"Erreur lors de l'affichage des commandes: {ex.Message}",
                                  "Erreur",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void ResetFields()
        {

            if (VoitureBox.Items.Count > 0)
                VoitureBox.SelectedIndex = 0;

            if (ClientBox.Items.Count > 0)
                ClientBox.SelectedIndex = 0;

            QuantiteNum.Value = 1;
            
        }


        private void CalculerPrixTotal()
        {
            if (VoitureBox.SelectedIndex != -1)
            {
                decimal prixUnitaire = GetPrixUnitaire(VoitureBox.Text);
                int quantite = Math.Max(1, (int)QuantiteNum.Value);
                decimal prixTotal = prixUnitaire * quantite;

                PrixTxt.Text = prixTotal.ToString("N2") + " Ar";
                PrixTxt.BackColor = Color.White;
                PrixTxt.Font = new Font(PrixTxt.Font, FontStyle.Bold);
            }
        }

        private void AfficherPrixUnitaire()
        {
            if (VoitureBox.SelectedIndex != -1)
            {
                decimal prixUnitaire = GetPrixUnitaire(VoitureBox.Text);
                CalculerPrixTotal();
            }
        }

        private void VoitureBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AfficherPrixUnitaire();
            CalculerPrixTotal();
        }

        private void QuantiteNum_ValueChanged(object sender, EventArgs e)
        {
            CalculerPrixTotal();
        }

       


        private void Recherchetxt_TextChanged(object sender, EventArgs e)
        {
            string recherche = Recherchetxt.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(recherche))
            {
                AfficherCommandes();
                return;
            }

            string selectQuery = @"SELECT IdCommande, voiture, client, quantite, prix, date FROM commande 
                                WHERE IdCommande LIKE @recherche 
                                OR LOWER(voiture) LIKE @recherche 
                                OR LOWER(client) LIKE @recherche";

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
    }
}