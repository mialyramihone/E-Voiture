using System;
using System.Data.SQLite;

namespace CFFAMMA
{
    internal class CFFAMMA_bd
    {
        private string connectionString;

        public CFFAMMA_bd(string databasePath)
        {

            connectionString = $"Data Source={databasePath};Version=3;";
        }

        private SQLiteConnection OpenConnection()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using (var command = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                command.ExecuteNonQuery();
            }

            return connection;
        }

        public void CreateTables()
        {
            using (var connection = OpenConnection())
            {
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS FILIERE (
                            ID_FILIERE INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS EQUIPEMENT_RURAL (
                            ID_EQUIP INTEGER PRIMARY KEY,
                            ID_FILIERE INTEGER,
                            FOREIGN KEY (ID_FILIERE) REFERENCES FILIERE (ID_FILIERE)
                        );

                        CREATE TABLE IF NOT EXISTS PAIEMENT (
                            ID_PAIEMENT INTEGER PRIMARY KEY,
                            ID_COMMANDE INTEGER,
                            FOREIGN KEY (ID_COMMANDE) REFERENCES COMMANDE (ID_COMMANDE)
                        );

                        CREATE TABLE IF NOT EXISTS COMMANDE (
                            ID_COMMANDE INTEGER PRIMARY KEY,
                            ID_CLIENT INTEGER,
                            FOREIGN KEY (ID_CLIENT) REFERENCES CLIENT (ID_CLIENT)
                        );

                        CREATE TABLE IF NOT EXISTS CLIENT (
                            ID_CLIENT INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS ETUDIANT (
                            ID_ETUDIANT INTEGER PRIMARY KEY,
                            ID_FILIERE INTEGER,
                            ID_VERSEMENT INTEGER,
                            ID_PROFESSION INTEGER,
                            ID_EVALUATION INTEGER,
                            ID_PROMOTION INTEGER,
                            ID_DIPLOME_ETUDIANT INTEGER,
                            FOREIGN KEY (ID_FILIERE) REFERENCES FILIERE (ID_FILIERE),
                            FOREIGN KEY (ID_VERSEMENT) REFERENCES VERSEMENT (ID_VERSEMENT),
                            FOREIGN KEY (ID_PROFESSION) REFERENCES PROFESSION_ETUDIANT (ID_PROFESSION),
                            FOREIGN KEY (ID_EVALUATION) REFERENCES EVALUATION (ID_EVALUATION),
                            FOREIGN KEY (ID_PROMOTION) REFERENCES PROMOTION_ETUDIANT (ID_PROMOTION),
                            FOREIGN KEY (ID_DIPLOME_ETUDIANT) REFERENCES DIPLOME_ETUDIANT (ID_DIPLOME_ETUDIANT)
                        );

                        CREATE TABLE IF NOT EXISTS VERSEMENT (
                            ID_VERSEMENT INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS PROFESSION_ETUDIANT (
                            ID_PROFESSION INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS EVALUATION (
                            ID_EVALUATION INTEGER PRIMARY KEY,
                            ID_MATIERE INTEGER,
                            FOREIGN KEY (ID_MATIERE) REFERENCES MATIERE (ID_MATIERE)
                        );

                        CREATE TABLE IF NOT EXISTS PROMOTION_ETUDIANT (
                            ID_PROMOTION INTEGER PRIMARY KEY,
                            ID_EVALUATION INTEGER,
                            FOREIGN KEY (ID_EVALUATION) REFERENCES EVALUATION (ID_EVALUATION)
                        );

                        CREATE TABLE IF NOT EXISTS DIPLOME_ETUDIANT (
                            ID_DIPLOME_ETUDIANT INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS CONGE (
                            ID_CONGE INTEGER PRIMARY KEY,
                            ID_DROIT INTEGER,
                            FOREIGN KEY (ID_DROIT) REFERENCES DROIT_CONGE (ID_DROIT)
                        );

                        CREATE TABLE IF NOT EXISTS DROIT_CONGE (
                            ID_DROIT INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS MACHINISME_AGRICOLE (
                            ID_MACHINISME INTEGER PRIMARY KEY,
                            ID_FILIERE INTEGER,
                            FOREIGN KEY (ID_FILIERE) REFERENCES FILIERE (ID_FILIERE)
                        );

                        CREATE TABLE IF NOT EXISTS DIRECTION (
                            ID_DIRECTION INTEGER PRIMARY KEY,
                            ID_SERVICE INTEGER,
                            FOREIGN KEY (ID_SERVICE) REFERENCES SERVICE (ID_SERVICE)
                        );

                        CREATE TABLE IF NOT EXISTS SERVICE (
                            ID_SERVICE INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS PERSONNEL (
                            ID_PERSONNEL INTEGER PRIMARY KEY,
                            ID_DIPLOME_PERSONNEL INTEGER,
                            ID_CONGE INTEGER,
                            ID_CONTRAT INTEGER,
                            ID_DEBAUCHAGE INTEGER,
                            ID_SERVICE INTEGER,
                            FOREIGN KEY (ID_DIPLOME_PERSONNEL) REFERENCES DIPLOME_PERSONNEL (ID_DIPLOME_PERSONNEL),
                            FOREIGN KEY (ID_CONGE) REFERENCES CONGE (ID_CONGE),
                            FOREIGN KEY (ID_CONTRAT) REFERENCES TYPE_CONTRAT (ID_CONTRAT),
                            FOREIGN KEY (ID_DEBAUCHAGE) REFERENCES DEMANDE_DEBAUCHAGE (ID_DEBAUCHAGE),
                            FOREIGN KEY (ID_SERVICE) REFERENCES SERVICE (ID_SERVICE)
                        );

                        CREATE TABLE IF NOT EXISTS DIPLOME_PERSONNEL (
                            ID_DIPLOME_PERSONNEL INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS TYPE_CONTRAT (
                            ID_CONTRAT INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS DEMANDE_DEBAUCHAGE (
                            ID_DEBAUCHAGE INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS MATERIEL (
                            ID_MATERIEL INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS PRODUCTION (
                            ID_PRODUCTION INTEGER PRIMARY KEY,
                            ID_MATERIEL INTEGER,
                            FOREIGN KEY (ID_MATERIEL) REFERENCES MATERIEL (ID_MATERIEL)
                        );

                        CREATE TABLE IF NOT EXISTS TECHNICIEN (
                            ID_TECHNICIEN INTEGER PRIMARY KEY,
                            ID_PERSONNEL INTEGER,
                            FOREIGN KEY (ID_PERSONNEL) REFERENCES PERSONNEL (ID_PERSONNEL)
                        );

                        CREATE TABLE IF NOT EXISTS BOURSE (
                            ID_BOURSE INTEGER PRIMARY KEY,
                            ID_ETUDIANT INTEGER,
                            FOREIGN KEY (ID_ETUDIANT) REFERENCES ETUDIANT (ID_ETUDIANT)
                        );

                        CREATE TABLE IF NOT EXISTS FINANCEMEMT_DON (
                            ID_FINANCEMENT INTEGER PRIMARY KEY,
                            ID_FICHE INTEGER,
                            FOREIGN KEY (ID_FICHE) REFERENCES FICHE_ENTRETIEN (ID_FICHE)
                        );

                        CREATE TABLE IF NOT EXISTS FICHE_ENTRETIEN (
                            ID_FICHE INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS MATIERE (
                            ID_MATIERE INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS RETRAIT (
                            ID_RETRAIT INTEGER PRIMARY KEY,
                            ID_PRODUIT INTEGER,
                            FOREIGN KEY (ID_PRODUIT) REFERENCES PRODUIT (ID_PRODUIT)
                        );

                        CREATE TABLE IF NOT EXISTS PRODUIT (
                            ID_PRODUIT INTEGER PRIMARY KEY
                        );

                        CREATE TABLE IF NOT EXISTS APPROVISIONNEMENT (
                            ID_APPROVISIONNEMENT INTEGER PRIMARY KEY,
                            ID_PRODUIT INTEGER,
                            FOREIGN KEY (ID_PRODUIT) REFERENCES PRODUIT (ID_PRODUIT)
                        );

                        CREATE TABLE IF NOT EXISTS ENSEIGNANT (
                            ID_ENSEIGNANT INTEGER PRIMARY KEY,
                            ID_PERSONNEL INTEGER,
                            FOREIGN KEY (ID_PERSONNEL) REFERENCES PERSONNEL (ID_PERSONNEL)
                        );

                        CREATE TABLE IF NOT EXISTS MISSION (
                            ID_MISSION INTEGER PRIMARY KEY,
                            ID_PERSONNEL INTEGER,
                            ID_MATERIEL INTEGER,
                            FOREIGN KEY (ID_PERSONNEL) REFERENCES PERSONNEL (ID_PERSONNEL),
                            FOREIGN KEY (ID_MATERIEL) REFERENCES MATERIEL (ID_MATERIEL)
                        );

                        CREATE TABLE IF NOT EXISTS FINANCEMENT_RP (
                            ID_FINANCEMENT INTEGER PRIMARY KEY,
                            ID_FICHE INTEGER,
                            FOREIGN KEY (ID_FICHE) REFERENCES FICHE_ENTRETIEN (ID_FICHE)
                        );

                        CREATE TABLE IF NOT EXISTS PEUT_�TRE_LI�_� (
                           ID_CONTRAT INTEGER(10) NOT NULL  ,
                           ID_DEBAUCHAGE INTEGER(10) NOT NULL  
                           , PRIMARY KEY (ID_CONTRAT,ID_DEBAUCHAGE) 
                         ) ;

                        CREATE TABLE IF NOT EXISTS GERER
                         (
                           ID_FICHE INTEGER(10) NOT NULL  ,
                           ID_PERSONNEL INTEGER(10) NOT NULL  
                           , PRIMARY KEY (ID_FICHE,ID_PERSONNEL) 
                         ) ;

                        CREATE TABLE IF NOT EXISTS UTILISER
                         (
                           ID_MATERIEL INTEGER(10) NOT NULL  ,
                           ID_FILIERE INTEGER(10) NOT NULL  
                           , PRIMARY KEY (ID_MATERIEL,ID_FILIERE) 
                         ) ;


                        CREATE TABLE IF NOT EXISTS ENSEIGNE
                         (
                           ID_PERSONNEL INTEGER(10) NOT NULL  ,
                           ID_MATIERE INTEGER(10) NOT NULL  
                           , PRIMARY KEY (ID_PERSONNEL,ID_MATIERE) 
                         ) ;

                        CREATE TABLE IF NOT EXISTS ASSOCIER_ENTRETIEN
                         (
                           ID_MATERIEL INTEGER(10) NOT NULL  ,
                           ID_FICHE INTEGER(10) NOT NULL  
                           , PRIMARY KEY (ID_MATERIEL,ID_FICHE) 
                         ) ;

                        CREATE TABLE IF NOT EXISTS APPARTENIR_�
                         (
                           ID_MATERIEL INTEGER(10) NOT NULL  ,
                           ID_PRODUIT INTEGER(2) NOT NULL  
                           , PRIMARY KEY (ID_MATERIEL,ID_PRODUIT) 
                         ) ;

                        CREATE TABLE IF NOT EXISTS AVOIR_COMMANDE
                         (
                           ID_COMMANDE INTEGER(10) NOT NULL  ,
                           ID_PRODUIT INTEGER(2) NOT NULL  
                           , PRIMARY KEY (ID_COMMANDE,ID_PRODUIT) 
                         ) 
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }
        public void AddForeignKeys(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = @"
            
                    ALTER TABLE EQUIPEMENT_RURAL 
                      ADD FOREIGN KEY FK_EQUIPEMENT_RURAL_FILIERE (ID_FILIERE)
                          REFERENCES FILIERE (ID_FILIERE) ;


                    ALTER TABLE PAIEMENT 
                      ADD FOREIGN KEY FK_PAIEMENT_COMMANDE (ID_COMMANDE)
                          REFERENCES COMMANDE (ID_COMMANDE) ;


                    ALTER TABLE ETUDIANT 
                      ADD FOREIGN KEY FK_ETUDIANT_VERSEMENT (ID_VERSEMENT)
                          REFERENCES VERSEMENT (ID_VERSEMENT) ;


                    ALTER TABLE ETUDIANT 
                      ADD FOREIGN KEY FK_ETUDIANT_FILIERE (ID_FILIERE)
                          REFERENCES FILIERE (ID_FILIERE) ;


                    ALTER TABLE ETUDIANT 
                      ADD FOREIGN KEY FK_ETUDIANT_PROFESSION_ETUDIANT (ID_PROFESSION)
                          REFERENCES PROFESSION_ETUDIANT (ID_PROFESSION) ;


                    ALTER TABLE ETUDIANT 
                      ADD FOREIGN KEY FK_ETUDIANT_EVALUATION (ID_EVALUATION)
                          REFERENCES EVALUATION (ID_EVALUATION) ;


                    ALTER TABLE ETUDIANT 
                      ADD FOREIGN KEY FK_ETUDIANT_PROMOTION_ETUDIANT (ID_PROMOTION)
                          REFERENCES PROMOTION_ETUDIANT (ID_PROMOTION) ;


                    ALTER TABLE ETUDIANT 
                      ADD FOREIGN KEY FK_ETUDIANT_DIPLOME_ETUDIANT (ID_DIPLOME_ETUDIANT)
                          REFERENCES DIPLOME_ETUDIANT (ID_DIPLOME_ETUDIANT) ;


                    ALTER TABLE COMMANDE 
                      ADD FOREIGN KEY FK_COMMANDE_CLIENT (ID_CLIENT)
                          REFERENCES CLIENT (ID_CLIENT) ;


                    ALTER TABLE CONGE 
                      ADD FOREIGN KEY FK_CONGE_DROIT_CONGE (ID_DROIT)
                          REFERENCES DROIT_CONGE (ID_DROIT) ;


                    ALTER TABLE MACHINISME_AGRICOLE 
                      ADD FOREIGN KEY FK_MACHINISME_AGRICOLE_FILIERE (ID_FILIERE)
                          REFERENCES FILIERE (ID_FILIERE) ;


                    ALTER TABLE DIRECTION 
                      ADD FOREIGN KEY FK_DIRECTION_SERVICE (ID_SERVICE)
                          REFERENCES SERVICE (ID_SERVICE) ;


                    ALTER TABLE PROMOTION_ETUDIANT 
                      ADD FOREIGN KEY FK_PROMOTION_ETUDIANT_EVALUATION (ID_EVALUATION)
                          REFERENCES EVALUATION (ID_EVALUATION) ;


                    ALTER TABLE PERSONNEL 
                      ADD FOREIGN KEY FK_PERSONNEL_DIPLOME_PERSONNEL (ID_DIPLOME_PERSONNEL)
                          REFERENCES DIPLOME_PERSONNEL (ID_DIPLOME_PERSONNEL) ;


                    ALTER TABLE PERSONNEL 
                      ADD FOREIGN KEY FK_PERSONNEL_CONGE (ID_CONGE)
                          REFERENCES CONGE (ID_CONGE) ;


                    ALTER TABLE PERSONNEL 
                      ADD FOREIGN KEY FK_PERSONNEL_TYPE_CONTRAT (ID_CONTRAT)
                          REFERENCES TYPE_CONTRAT (ID_CONTRAT) ;


                    ALTER TABLE PERSONNEL 
                      ADD FOREIGN KEY FK_PERSONNEL_DEMANDE_DEBAUCHAGE (ID_DEBAUCHAGE)
                          REFERENCES DEMANDE_DEBAUCHAGE (ID_DEBAUCHAGE) ;


                    ALTER TABLE PERSONNEL 
                      ADD FOREIGN KEY FK_PERSONNEL_SERVICE (ID_SERVICE)
                          REFERENCES SERVICE (ID_SERVICE) ;


                    ALTER TABLE PRODUCTION 
                      ADD FOREIGN KEY FK_PRODUCTION_MATERIEL (ID_MATERIEL)
                          REFERENCES MATERIEL (ID_MATERIEL) ;


                    ALTER TABLE TECHNICIEN 
                      ADD FOREIGN KEY FK_TECHNICIEN_PERSONNEL (ID_PERSONNEL)
                          REFERENCES PERSONNEL (ID_PERSONNEL) ;


                    ALTER TABLE BOURSE 
                      ADD FOREIGN KEY FK_BOURSE_ETUDIANT (ID_ETUDIANT)
                          REFERENCES ETUDIANT (ID_ETUDIANT) ;


                    ALTER TABLE FINANCEMEMT_DON 
                      ADD FOREIGN KEY FK_FINANCEMEMT_DON_FICHE_ENTRETIEN (ID_FICHE)
                          REFERENCES FICHE_ENTRETIEN (ID_FICHE) ;


                    ALTER TABLE EVALUATION 
                      ADD FOREIGN KEY FK_EVALUATION_MATIERE (ID_MATIERE)
                          REFERENCES MATIERE (ID_MATIERE) ;


                    ALTER TABLE RETRAIT 
                      ADD FOREIGN KEY FK_RETRAIT_PRODUIT (ID_PRODUIT)
                          REFERENCES PRODUIT (ID_PRODUIT) ;


                    ALTER TABLE APPROVISIONNEMENT 
                      ADD FOREIGN KEY FK_APPROVISIONNEMENT_PRODUIT (ID_PRODUIT)
                          REFERENCES PRODUIT (ID_PRODUIT) ;


                    ALTER TABLE ENSEIGNANT 
                      ADD FOREIGN KEY FK_ENSEIGNANT_PERSONNEL (ID_PERSONNEL)
                          REFERENCES PERSONNEL (ID_PERSONNEL) ;


                    ALTER TABLE MISSION 
                      ADD FOREIGN KEY FK_MISSION_PERSONNEL (ID_PERSONNEL)
                          REFERENCES PERSONNEL (ID_PERSONNEL) ;


                    ALTER TABLE MISSION 
                      ADD FOREIGN KEY FK_MISSION_MATERIEL (ID_MATERIEL)
                          REFERENCES MATERIEL (ID_MATERIEL) ;


                    ALTER TABLE FINANCEMENT_RP 
                      ADD FOREIGN KEY FK_FINANCEMENT_RP_FICHE_ENTRETIEN (ID_FICHE)
                          REFERENCES FICHE_ENTRETIEN (ID_FICHE) ;


                    ALTER TABLE PEUT_�TRE_LI�_� 
                      ADD FOREIGN KEY FK_PEUT_�TRE_LI�_�_TYPE_CONTRAT (ID_CONTRAT)
                          REFERENCES TYPE_CONTRAT (ID_CONTRAT) ;


                    ALTER TABLE PEUT_�TRE_LI�_� 
                      ADD FOREIGN KEY FK_PEUT_�TRE_LI�_�_DEMANDE_DEBAUCHAGE (ID_DEBAUCHAGE)
                          REFERENCES DEMANDE_DEBAUCHAGE (ID_DEBAUCHAGE) ;


                    ALTER TABLE GERER 
                      ADD FOREIGN KEY FK_GERER_FICHE_ENTRETIEN (ID_FICHE)
                          REFERENCES FICHE_ENTRETIEN (ID_FICHE) ;


                    ALTER TABLE GERER 
                      ADD FOREIGN KEY FK_GERER_TECHNICIEN (ID_PERSONNEL)
                          REFERENCES TECHNICIEN (ID_PERSONNEL) ;


                    ALTER TABLE UTILISER 
                      ADD FOREIGN KEY FK_UTILISER_MATERIEL (ID_MATERIEL)
                          REFERENCES MATERIEL (ID_MATERIEL) ;


                    ALTER TABLE UTILISER 
                      ADD FOREIGN KEY FK_UTILISER_MACHINISME_AGRICOLE (ID_FILIERE)
                          REFERENCES MACHINISME_AGRICOLE (ID_FILIERE) ;


                    ALTER TABLE ENSEIGNE 
                      ADD FOREIGN KEY FK_ENSEIGNE_ENSEIGNANT (ID_PERSONNEL)
                          REFERENCES ENSEIGNANT (ID_PERSONNEL) ;


                    ALTER TABLE ENSEIGNE 
                      ADD FOREIGN KEY FK_ENSEIGNE_MATIERE (ID_MATIERE)
                          REFERENCES MATIERE (ID_MATIERE) ;


                    ALTER TABLE ASSOCIER_ENTRETIEN 
                      ADD FOREIGN KEY FK_ASSOCIER_ENTRETIEN_MATERIEL (ID_MATERIEL)
                          REFERENCES MATERIEL (ID_MATERIEL) ;


                    ALTER TABLE ASSOCIER_ENTRETIEN 
                      ADD FOREIGN KEY FK_ASSOCIER_ENTRETIEN_FICHE_ENTRETIEN (ID_FICHE)
                          REFERENCES FICHE_ENTRETIEN (ID_FICHE) ;


                    ALTER TABLE APPARTENIR_� 
                      ADD FOREIGN KEY FK_APPARTENIR_�_MATERIEL (ID_MATERIEL)
                          REFERENCES MATERIEL (ID_MATERIEL) ;


                    ALTER TABLE APPARTENIR_� 
                      ADD FOREIGN KEY FK_APPARTENIR_�_PRODUIT (ID_PRODUIT)
                          REFERENCES PRODUIT (ID_PRODUIT) ;


                    ALTER TABLE AVOIR_COMMANDE 
                      ADD FOREIGN KEY FK_AVOIR_COMMANDE_COMMANDE (ID_COMMANDE)
                          REFERENCES COMMANDE (ID_COMMANDE) ;


                    ALTER TABLE AVOIR_COMMANDE 
                      ADD FOREIGN KEY FK_AVOIR_COMMANDE_PRODUIT (ID_PRODUIT)
                          REFERENCES PRODUIT (ID_PRODUIT) ;


        ";
                command.ExecuteNonQuery();
            }
        }
    }

}
