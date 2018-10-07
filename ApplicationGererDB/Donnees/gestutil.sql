-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le :  Dim 07 oct. 2018 à 19:03
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `identifiants`
--

INSERT INTO `identifiants` (`id`, `login`, `mdp`) VALUES
(1, 'Gtlcl17', 'virer45'),
(2, 'Tovae6w11', 'philip12');

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
  `ref_identifiant` int(11) NOT NULL,
  `nom` varchar(32) NOT NULL,
  `prénom` varchar(32) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `nom_2` (`nom`,`prénom`),
  KEY `ref_id` (`ref_identifiant`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `utilisateur`
--

INSERT INTO `utilisateur` (`id`, `ref_identifiant`, `nom`, `prénom`) VALUES
(1, 1, 'Toller', 'Guillaume'),
(3, 2, 'Vervaet', 'Tommy');

-- --------------------------------------------------------

--
-- Structure de la table `utilisateur_role`
--

DROP TABLE IF EXISTS `utilisateur_role`;
CREATE TABLE IF NOT EXISTS `utilisateur_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ref_utilisateur` int(11) NOT NULL,
  `ref_role` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ref_identifiant` (`ref_utilisateur`),
  KEY `ref_role` (`ref_role`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `utilisateur_role`
--

INSERT INTO `utilisateur_role` (`id`, `ref_utilisateur`, `ref_role`) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 1, 3),
(4, 3, 2),
(5, 3, 3);

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `utilisateur`
--
ALTER TABLE `utilisateur`
  ADD CONSTRAINT `identifiant_ref_identifiant` FOREIGN KEY (`ref_identifiant`) REFERENCES `identifiants` (`id`);

--
-- Contraintes pour la table `utilisateur_role`
--
ALTER TABLE `utilisateur_role`
  ADD CONSTRAINT `role_refrole` FOREIGN KEY (`ref_role`) REFERENCES `roles` (`id`),
  ADD CONSTRAINT `utilisateur_refutilisateur` FOREIGN KEY (`ref_utilisateur`) REFERENCES `utilisateur` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
