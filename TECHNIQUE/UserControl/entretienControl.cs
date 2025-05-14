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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CFFAMMA.TECHNIQUE
{
    public partial class entretienControl : UserControl
    {
        public entretienControl()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            textBoxProvenance.Enabled = false;

            radioButtonRP.CheckedChanged += RadioButton_CheckedChanged;
            radioButtonDON.CheckedChanged += RadioButton_CheckedChanged;


        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRP.Checked)
            {

                textBoxProvenance.Enabled = false;
                textBoxProvenance.Text = string.Empty; 
            }
            else if (radioButtonDON.Checked)
            {

                textBoxProvenance.Enabled = true;
            }
        }

        private string connectionString = "Server=localhost;Database=mlr1;Uid=root;Pwd=;";

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void refP_TextChanged(object sender, EventArgs e)
        {
        }

        private void ajoutBtn_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(refclient.Text) ||
            //    string.IsNullOrEmpty(nomClient.Text) ||
            //    string.IsNullOrEmpty(telephone.Text) ||
            //    string.IsNullOrEmpty(email.Text) ||
            //    string.IsNullOrEmpty(adresse.Text))
            //{
            //    MessageBox.Show("Veuillez remplir tous les champs correctement.", "Champs manquants", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //string idClient = refclient.Text.Trim();
            //string nomClientValue = nomClient.Text.Trim();
            //string telephoneClient = telephone.Text.Trim();
            //string emailClient = email.Text.Trim();
            //string adresseClient = adresse.Text.Trim();

            //string insertQuery = "INSERT INTO client(ID_CLIENT, NOM_CLIENT, TEL_CLIENT, EMAIL_CLIENT, ADRESSE_CLIENT) " +
            //                     "VALUES (@idClient, @nomClient, @telephone, @email, @adresse)";

            //using (MySqlConnection connection = new MySqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();

            //        using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            //        {
            //            command.Parameters.AddWithValue("@idClient", idClient);
            //            command.Parameters.AddWithValue("@nomClient", nomClientValue);
            //            command.Parameters.AddWithValue("@telephone", telephoneClient);
            //            command.Parameters.AddWithValue("@email", emailClient);
            //            command.Parameters.AddWithValue("@adresse", adresseClient);

            //            int rowsAffected = command.ExecuteNonQuery();

            //            if (rowsAffected > 0)
            //            {
            //                MessageBox.Show("Client ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                AfficherClients();
            //                ResetFields();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Erreur lors de l'ajout du client.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Erreur de connexion ou d'insertion : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void afficherBtn_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            //    refclient.Text = selectedRow.Cells["ID_CLIENT"].Value?.ToString();
            //    nomClient.Text = selectedRow.Cells["NOM_CLIENT"].Value?.ToString();
            //    telephone.Text = selectedRow.Cells["TEL_CLIENT"].Value?.ToString();
            //    email.Text = selectedRow.Cells["EMAIL_CLIENT"].Value?.ToString();
            //    adresse.Text = selectedRow.Cells["ADRESSE_CLIENT"].Value?.ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Veuillez sélectionner une ligne dans le tableau.");
            //}
        }

        private void majBtn_Click(object sender, EventArgs e)
        {
            //string refClient = refclient.Text;
            //string nomClientText = nomClient.Text;
            //string telephoneClient = telephone.Text;
            //string emailClient = email.Text;
            //string adresseClient = adresse.Text;

            //if (string.IsNullOrEmpty(refClient) || string.IsNullOrEmpty(nomClientText) ||
            //    string.IsNullOrEmpty(telephoneClient) || string.IsNullOrEmpty(emailClient) || string.IsNullOrEmpty(adresseClient))
            //{
            //    MessageBox.Show("Veuillez remplir tous les champs correctement.");
            //    return;
            //}

            //string updateQuery = "UPDATE client SET NOM_CLIENT = @nomClient, TEL_CLIENT = @telephone, " +
            //                     "EMAIL_CLIENT = @email, ADRESSE_CLIENT = @adresse WHERE ID_CLIENT = @refClient";

            //using (MySqlConnection connection = new MySqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();

            //        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
            //        {
            //            command.Parameters.AddWithValue("@refClient", refClient);
            //            command.Parameters.AddWithValue("@nomClient", nomClientText);
            //            command.Parameters.AddWithValue("@telephone", telephoneClient);
            //            command.Parameters.AddWithValue("@email", emailClient);
            //            command.Parameters.AddWithValue("@adresse", adresseClient);

            //            int rowsAffected = command.ExecuteNonQuery();
            //            if (rowsAffected > 0)
            //            {
            //                MessageBox.Show("Client mis à jour avec succès !");
            //                AfficherClients();
            //                ResetFields();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Aucun client trouvé avec cette référence.");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}");
            //    }
            //}
        }

        private void supBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
                return;
            }

            string idClient = dataGridView1.SelectedRows[0].Cells["ID_CLIENT"].Value?.ToString();

            if (string.IsNullOrEmpty(idClient))
            {
                MessageBox.Show("Impossible de récupérer l'ID du client. Veuillez réessayer.");
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce client ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation == DialogResult.No)
            {
                return;
            }

            string deleteQuery = "DELETE FROM client WHERE ID_CLIENT = @idClient";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idClient", idClient);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Client supprimé avec succès !");
                            //AfficherClients();
                        }
                        else
                        {
                            MessageBox.Show("Aucun client trouvé avec cet ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}");
                }
            }
        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
