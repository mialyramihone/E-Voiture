-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : mer. 14 mai 2025 à 15:24
-- Version du serveur : 8.2.0
-- Version de PHP : 8.2.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `gestionvoiture`
--

-- --------------------------------------------------------

--
-- Structure de la table `client`
--

DROP TABLE IF EXISTS `client`;
CREATE TABLE IF NOT EXISTS `client` (
  `IdClient` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(250) NOT NULL,
  `prenom` varchar(250) NOT NULL,
  `numero` varchar(250) NOT NULL,
  `email` varchar(250) NOT NULL,
  `adresse` varchar(250) NOT NULL,
  PRIMARY KEY (`IdClient`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `client`
--

INSERT INTO `client` (`IdClient`, `nom`, `prenom`, `numero`, `email`, `adresse`) VALUES
(1, 'RAKOTO', 'Jean', '034 12 345 67', 'jean.rakoto@email.com', 'Antananarivo, 101'),
(2, 'RASOLO', 'Miora', '032 78 900 12', 'miora.rasolo@email.com', 'Toamasina, quartier Bazary'),
(3, 'RANDRIAM', 'Solo', '033 45 678 91', 'solo.randriam@email.com', 'Fianarantsoa, Ambalavao'),
(4, 'RAZAFI', 'Heriniaina', '034 89 123 45', 'herin.razafi@email.com', 'Majunga, Tsaramandroso'),
(5, 'ANDRIANA', 'Fanja', '032 34 567 89', 'fanja.andriana@email.com', 'Antsirabe, Mahazoarivo'),
(6, 'RAKOTOM', 'Mickael', '033 98 765 43', 'micka.rakotom@email.com', 'Antananarivo, Ankorondrano'),
(7, 'RAZANA', 'Lova', '034 76 543 21', 'lova.razana@email.com', 'Toliara, quartier Andaboly'),
(8, 'ANDRY', 'Tiana', '032 65 432 10', 'tiana.andry@email.com', 'Morondava, centre-ville'),
(9, 'ZAFIMAH', 'Tsiory', '033 22 334 45', 'tsiory.zafimah@email.com', 'Sambava, proche du marché'),
(10, 'RAMANAN', 'Elia', '034 67 891 23', 'elia.ramanan@email.com', 'Manakara, Soanierana');

-- --------------------------------------------------------

--
-- Structure de la table `commande`
--

DROP TABLE IF EXISTS `commande`;
CREATE TABLE IF NOT EXISTS `commande` (
  `IdCommande` int NOT NULL AUTO_INCREMENT,
  `voiture` varchar(250) NOT NULL,
  `client` varchar(250) NOT NULL,
  `quantite` varchar(250) NOT NULL,
  `prix` varchar(250) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`IdCommande`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `commande`
--

INSERT INTO `commande` (`IdCommande`, `voiture`, `client`, `quantite`, `prix`, `date`) VALUES
(2, 'Peugeot 208 - Bleu', 'RANDRIAM Solo', '10', '290000000', '2025-05-14'),
(1, 'Mitsubishi Pajero - Blanc', 'RASOLO Miora', '2', '110000000', '2025-05-14'),
(3, 'Mitsubishi Pajero - Blanc', 'RAZAFI Heriniaina', '3', '165000000', '2025-05-14'),
(4, 'Peugeot 208 - Bleu', 'ANDRIANA Fanja', '7', '203000000', '2025-05-14'),
(5, 'Kia Sportage - Rouge', 'RAZANA Lova', '3', '135000000', '2025-05-14'),
(6, 'Peugeot 208 - Bleu', 'RAZAFI Heriniaina', '3', '87000000', '2025-05-14'),
(7, 'Ford Ranger - Gris', 'RAMANAN Elia', '4', '240000000', '2025-05-14'),
(8, 'Peugeot 208 - Bleu', 'ANDRY Tiana', '9', '261000000', '2025-05-14'),
(9, 'Toyota Corolla - Blanc', 'RANDRIAM Solo', '10', '350000000', '2025-05-14'),
(10, 'Renault Clio - Jaune', 'RAKOTOM Mickael', '5', '155000000', '2025-05-14');

-- --------------------------------------------------------

--
-- Structure de la table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
CREATE TABLE IF NOT EXISTS `livraison` (
  `IdCommande` int NOT NULL AUTO_INCREMENT,
  `voiture` varchar(250) NOT NULL,
  `client` varchar(250) NOT NULL,
  `quantite` varchar(250) NOT NULL,
  `prixlivraison` varchar(250) NOT NULL,
  `montant` varchar(250) NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`IdCommande`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `livraison`
--

INSERT INTO `livraison` (`IdCommande`, `voiture`, `client`, `quantite`, `prixlivraison`, `montant`, `date`) VALUES
(1, 'Mitsubishi Pajero - Blanc', 'RASOLO Miora', '2', '40000', '110040000.00', '2025-05-14'),
(4, 'Peugeot 208 - Bleu', 'ANDRIANA Fanja', '7', '30000', '203030000.00', '2025-05-14'),
(6, 'Peugeot 208 - Bleu', 'RAZAFI Heriniaina', '3', '4000', '87004000.00', '2025-05-14'),
(8, 'Peugeot 208 - Bleu', 'ANDRY Tiana', '9', '60000', '261060000.00', '2025-05-14'),
(10, 'Renault Clio - Jaune', 'RAKOTOM Mickael', '5', '90000', '155090000.00', '2025-05-14');

-- --------------------------------------------------------

--
-- Structure de la table `voiture`
--

DROP TABLE IF EXISTS `voiture`;
CREATE TABLE IF NOT EXISTS `voiture` (
  `IdVoiture` int NOT NULL AUTO_INCREMENT,
  `marque` varchar(250) NOT NULL,
  `modele` varchar(250) NOT NULL,
  `couleur` varchar(250) NOT NULL,
  `prix` varchar(250) NOT NULL,
  `annee` date NOT NULL,
  PRIMARY KEY (`IdVoiture`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `voiture`
--

INSERT INTO `voiture` (`IdVoiture`, `marque`, `modele`, `couleur`, `prix`, `annee`) VALUES
(2, 'Nissan', 'Qashqai', 'Gris', '42500000', '2022-01-01'),
(1, 'Toyota', 'Corolla', 'Blanc', '35000000', '2020-01-01'),
(3, 'Hyundai', 'Tucson', 'Noir', '48000000', '2022-01-01'),
(4, 'Peugeot', '208', 'Bleu', '29000000', '2019-01-01'),
(5, 'Kia', 'Sportage', 'Rouge', '45000000', '2023-01-01'),
(6, 'Mitsubishi', 'Pajero', 'Blanc', '55000000', '2020-05-14'),
(7, 'Renault', 'Clio', 'Jaune', '31000000', '2021-01-01'),
(8, 'Ford', 'Ranger', 'Gris', '60000000', '2022-03-03'),
(9, 'BMW', 'Série 3', 'Mauve', '75000000', '2021-02-05'),
(10, 'Mercedes', 'Classe A', 'Argent', '60000000', '2020-05-14');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
