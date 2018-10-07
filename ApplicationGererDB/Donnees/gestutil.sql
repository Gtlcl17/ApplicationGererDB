-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le :  Dim 07 oct. 2018 à 20:03
-- Version du serveur :  5.7.19
-- Version de PHP :  5.6.31

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données :  `gestutil`
--
CREATE DATABASE IF NOT EXISTS `gestutil` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `gestutil`;

-- --------------------------------------------------------

--
-- Structure de la table `identifiants`
--

DROP TABLE IF EXISTS `identifiants`;
CREATE TABLE IF NOT EXISTS `identifiants` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(32) NOT NULL,
  `mdp` varchar(32) NOT NULL,
  `ref_utilisateur` int(11) NOT NULL,
  `ref_role` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ref_utilisateur` (`ref_utilisateur`),
  KEY `ref_role` (`ref_role`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `identifiants`
--

INSERT INTO `identifiants` (`id`, `login`, `mdp`, `ref_utilisateur`, `ref_role`) VALUES
(1, 'Gtlcl17', 'virer45', 1, 1),
(2, 'Tovae6w11', 'philip12', 3, 3);

-- --------------------------------------------------------

--
-- Structure de la table `roles`
--

DROP TABLE IF EXISTS `roles`;
CREATE TABLE IF NOT EXISTS `roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nom` varchar(32) NOT NULL,
  `description` varchar(128) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `roles`
--

INSERT INTO `roles` (`id`, `nom`, `description`) VALUES
(1, 'administrateur', 'administrateur de l\'application'),
(2, 'spécialiste', 'spécialiste de cette application'),
(3, 'joueur', 'joueur de cette application');

-- --------------------------------------------------------

--
-- Structure de la table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
CREATE TABLE IF NOT EXISTS `utilisateur` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nom` varchar(32) NOT NULL,
  `prénom` varchar(32) NOT NULL,
  `email` varchar(64) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `nom_2` (`nom`,`prénom`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `utilisateur`
--

INSERT INTO `utilisateur` (`id`, `nom`, `prénom`, `email`) VALUES
(1, 'Toller', 'Guillaume', 'tollerguillaume@hotmail.com'),
(3, 'Vervaet', 'Tommy', 'vervaettommy@hotmail.com');

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `identifiants`
--
ALTER TABLE `identifiants`
  ADD CONSTRAINT `role_refrole` FOREIGN KEY (`ref_role`) REFERENCES `roles` (`id`),
  ADD CONSTRAINT `utilisateur_refutilisateur` FOREIGN KEY (`ref_utilisateur`) REFERENCES `utilisateur` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
