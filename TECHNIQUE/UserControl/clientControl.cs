using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using static CFFAMMA.TECHNIQUE.homeControl;

namespace CFFAMMA.TECHNIQUE
{
    public partial class clientControl : UserControl
    {
        private const string ConnectionString = "Server=localhost;Database=gestionvoiture;Uid=root;Pwd=;";

        public clientControl()
        {
            InitializeComponent();
            ConfigureDataGridView();
            //InitializePlaceholder();
            LoadClients();


            PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche par ID, nom ou prenom");
            Recherchetxt.TextChanged += new EventHandler(Recherchetxt_TextChanged);
        }


        private void Recherchetxt_TextChanged(object sender, EventArgs e)
        {
            string recherche = Recherchetxt.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(recherche))
            {
                LoadClients();
                return;
            }

            string selectQuery = @"SELECT IdClient, nom, prenom, numero, email, adresse FROM client 
                                WHERE IdClient LIKE @recherche 
                                OR LOWER(nom) LIKE @recherche 
                                OR LOWER(prenom) LIKE @recherche";

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
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

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }

        //private void InitializePlaceholder()
        //{

        //    PlaceholderHelper.SetPlaceholder(Recherchetxt, "Recherche");
        //}




        private void LoadClients()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM client ORDER BY IdClient";
                    var adapter = new MySqlDataAdapter(query, connection);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur lors du chargement des clients: {ex.Message}");
            }
        }

        private void ajoutBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateClientFields()) return;

            var idClient = IdClient.Text.Trim();
            if (!int.TryParse(idClient, out _))
            {
                ShowError("L'ID client doit être un nombre valide.");
                return;
            }

            if (ClientExists(idClient))
            {
                ShowError("Cet ID client existe déjà.");
                return;
            }

            if (InsertClient(idClient, NomTxt.Text.Trim(), PrenomTxt.Text.Trim(),
                           NumeroTxt.Text.Trim(), EmailTxt.Text.Trim(), AdresseTxt.Text.Trim()))
            {
                ShowSuccess("Client ajouté avec succès.");
                LoadClients();
                ResetFields();
            }
        }

        private bool ValidateClientFields()
        {
            if (string.IsNullOrWhiteSpace(IdClient.Text) ||
                string.IsNullOrWhiteSpace(NomTxt.Text) ||
                string.IsNullOrWhiteSpace(PrenomTxt.Text) ||
                string.IsNullOrWhiteSpace(NumeroTxt.Text) ||
                string.IsNullOrWhiteSpace(EmailTxt.Text) ||
                string.IsNullOrWhiteSpace(AdresseTxt.Text))
            {
                ShowWarning("Veuillez remplir tous les champs.");
                return false;
            }
            return true;
        }

        private bool ClientExists(string idClient)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT COUNT(*) FROM client WHERE IdClient = @id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", idClient);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private bool InsertClient(string id, string nom, string prenom, string telephone, string email, string adresse)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var query = @"INSERT INTO client(IdClient, nom, prenom, numero, email, adresse) 
                                 VALUES (@id, @nom, @prenom, @tel, @email, @adresse)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@nom", nom);
                        command.Parameters.AddWithValue("@prenom", prenom);
                        command.Parameters.AddWithValue("@tel", telephone);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@adresse", adresse);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur lors de l'ajout: {ex.Message}");
                return false;
            }
        }

        private void majBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IdClient.Text))
            {
                ShowWarning("Veuillez sélectionner un client à modifier.");
                return;
            }

            if (!ValidateClientFields()) return;

            var idClient = IdClient.Text.Trim();
            if (UpdateClient(idClient, NomTxt.Text.Trim(), PrenomTxt.Text.Trim(),
                            NumeroTxt.Text.Trim(), EmailTxt.Text.Trim(), AdresseTxt.Text.Trim()))
            {
                ShowSuccess("Client mis à jour avec succès.");
                LoadClients();
                ResetFields(); 
            }
        }

        private bool UpdateClient(string id, string nom, string prenom, string telephone, string email, string adresse)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var query = @"UPDATE client SET 
                                nom = @nom, 
                                prenom = @prenom, 
                                numero = @tel, 
                                email = @email, 
                                adresse = @adresse 
                                WHERE IdClient = @id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@nom", nom);
                        command.Parameters.AddWithValue("@prenom", prenom);
                        command.Parameters.AddWithValue("@tel", telephone);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@adresse", adresse);

                        var rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            ShowError("Aucun client trouvé avec cet ID.");
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur lors de la mise à jour: {ex.Message}");
                return false;
            }
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                ShowWarning("Veuillez sélectionner un client.");
                return;
            }

            var idClient = dataGridView1.SelectedRows[0].Cells["IdClient"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(idClient))
            {
                ShowError("Impossible de récupérer l'ID du client.");
                return;
            }

            if (MessageBox.Show("Confirmez la suppression ?", "Confirmation",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (DeleteClient(idClient))
            {
                ShowSuccess("Client supprimé avec succès.");
                LoadClients();
                ResetFields();
            }
        }

        private bool DeleteClient(string idClient)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM client WHERE IdClient = @id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", idClient);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur lors de la suppression: {ex.Message}");
                return false;
            }
        }

        private void afficherBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                DisplayClientData(row);
            }
        }

        private void DisplayClientData(DataGridViewRow row)
        {
            IdClient.Text = row.Cells["IdClient"].Value?.ToString();
            NomTxt.Text = row.Cells["nom"].Value?.ToString();
            PrenomTxt.Text = row.Cells["prenom"].Value?.ToString();
            NumeroTxt.Text = row.Cells["numero"].Value?.ToString();
            EmailTxt.Text = row.Cells["email"].Value?.ToString();
            AdresseTxt.Text = row.Cells["adresse"].Value?.ToString();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DisplayClientData(dataGridView1.SelectedRows[0]);
            }
        }

        private void ResetFields()
        {
            IdClient.Clear();
            NomTxt.Clear();
            PrenomTxt.Clear();
            NumeroTxt.Clear();
            EmailTxt.Clear();
            AdresseTxt.Clear();
        }

        #region Helper Methods
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}