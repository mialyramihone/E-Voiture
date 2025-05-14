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

namespace CFFAMMA.TECHNIQUE
{
    public partial class stockControl : UserControl
    {
        public stockControl()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadData();

        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    string queryProduit = "SELECT `ID_PRODUIT`, `TYPE_PRODUIT`, `NOM_PRODUIT`, `PRIX_UNITAIRE` FROM produit";
                    MySqlDataAdapter adapterProduit = new MySqlDataAdapter(queryProduit, connection);
                    DataTable dataProduit = new DataTable();
                    adapterProduit.Fill(dataProduit);


                    string queryApprovisionnement = "SELECT `ID_PRODUIT`, `TYPE_PRODUIT`, `NOM_PRODUIT`, `QUANTITE_ENTREE`, `DATE_ENTREE` FROM approvisionnement";
                    MySqlDataAdapter adapterApprovisionnement = new MySqlDataAdapter(queryApprovisionnement, connection);
                    DataTable dataApprovisionnement = new DataTable();
                    adapterApprovisionnement.Fill(dataApprovisionnement);


                    string queryRetrait = "SELECT `ID_PRODUIT`, `TYPE_PRODUIT`, `NOM_PRODUIT`, `DATE_SORTIE`, `QUANTITE_SORTIE` FROM retrait";
                    MySqlDataAdapter adapterRetrait = new MySqlDataAdapter(queryRetrait, connection);
                    DataTable dataRetrait = new DataTable();
                    adapterRetrait.Fill(dataRetrait);


                    DataTable mergedData = new DataTable();


                    mergedData.Columns.Add("ID_PRODUIT", typeof(string));
                    mergedData.Columns.Add("TYPE_PRODUIT", typeof(string));
                    mergedData.Columns.Add("NOM_PRODUIT", typeof(string));
                    mergedData.Columns.Add("PRIX_UNITAIRE", typeof(decimal));
                    mergedData.Columns.Add("QUANTITE_STOCK", typeof(int));
                    mergedData.Columns.Add("QUANTITE_ENTREE", typeof(int));
                    mergedData.Columns.Add("DATE_ENTREE", typeof(DateTime));
                    mergedData.Columns.Add("QUANTITE_SORTIE", typeof(int));
                    mergedData.Columns.Add("DATE_SORTIE", typeof(DateTime));


                    foreach (DataRow row in dataProduit.Rows)
                    {
                        mergedData.Rows.Add(
                            row["ID_PRODUIT"],
                            row["TYPE_PRODUIT"],
                            row["NOM_PRODUIT"],
                            row["PRIX_UNITAIRE"],
                            DBNull.Value, 
                            DBNull.Value, 
                            DBNull.Value,
                            DBNull.Value, 
                            DBNull.Value  
                        );
                    }


                    foreach (DataRow row in dataApprovisionnement.Rows)
                    {
                        mergedData.Rows.Add(
                            row["ID_PRODUIT"],
                            row["TYPE_PRODUIT"],
                            row["NOM_PRODUIT"],
                            DBNull.Value,
                            DBNull.Value,
                            row["QUANTITE_ENTREE"],
                            row["DATE_ENTREE"],
                            DBNull.Value,
                            DBNull.Value, 
                            DBNull.Value
                        );
                    }


                    foreach (DataRow row in dataRetrait.Rows)
                    {
                        mergedData.Rows.Add(
                            row["ID_PRODUIT"],
                            row["TYPE_PRODUIT"],
                            row["NOM_PRODUIT"],
                            DBNull.Value, 
                            DBNull.Value,
                            DBNull.Value,
                            row["QUANTITE_SORTIE"],
                            row["DATE_SORTIE"]
                        );
                    }

                    dataGridView1.DataSource = mergedData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private string connectionString = "Server=localhost;Database=mlr1;Uid=root;Pwd=;";

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        
    }
}
