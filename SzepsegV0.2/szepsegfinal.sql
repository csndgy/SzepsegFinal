-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 13, 2024 at 10:22 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `szepsegfinal`
--

-- --------------------------------------------------------

--
-- Table structure for table `dolgozók`
--

CREATE TABLE `dolgozók` (
  `dolgozoID` int(11) NOT NULL,
  `dolgozoFirstName` varchar(255) DEFAULT NULL,
  `dolgozoLastName` varchar(255) DEFAULT NULL,
  `dolgozoTel` varchar(20) DEFAULT NULL,
  `dolgozoEmail` varchar(255) DEFAULT NULL,
  `statusz` tinyint(1) DEFAULT NULL,
  `szolgáltatasa` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `dolgozók`
--

INSERT INTO `dolgozók` (`dolgozoID`, `dolgozoFirstName`, `dolgozoLastName`, `dolgozoTel`, `dolgozoEmail`, `statusz`, `szolgáltatasa`) VALUES
(1, 'Márk', 'Kiss', '+3612348765', 'mark.kiss@example.com', 1, 1),
(2, 'Sára', 'Horváth', '+3622348765', 'sara.horvath@example.com', 1, 2),
(3, 'János', 'Farkas', '+3632348765', 'janos.farkas@example.com', 0, 3),
(4, 'Dóra', 'Németh', '+3642348765', 'dora.nemeth@example.com', 1, 1),
(5, 'Ádám', 'Papp', '+3652348765', 'adam.papp@example.com', 0, 2),
(6, 'András', 'Tóth', '+3612345678', 'andras.toth@example.com', 1, 3),
(7, 'Márta', 'Kovács', '+3622345678', 'marta.kovacs@example.com', 0, 4),
(8, 'Erika', 'Szabó', '+3632345678', 'erika.szabo@example.com', 1, 2),
(9, 'Lajos', 'Takács', '+3642345678', 'lajos.takacs@example.com', 0, 5),
(10, 'Petra', 'Varga', '+3652345678', 'petra.varga@example.com', 1, 1),
(11, 'Katalin', 'Fekete', '+3662345678', 'katalin.fekete@example.com', 1, 2),
(12, 'Bence', 'Balog', '+3672345678', 'bence.balog@example.com', 0, 3),
(13, 'Gábor', 'Szilágyi', '+3682345678', 'gabor.szilagyi@example.com', 1, 4),
(14, 'Zoltán', 'Juhász', '+3692345678', 'zoltan.juhasz@example.com', 0, 5),
(15, 'Lilla', 'Váradi', '+3612345678', 'lilla.varadi@example.com', 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `foglalás`
--

CREATE TABLE `foglalás` (
  `foglalasID` int(11) NOT NULL,
  `szolgaltatasID` int(11) DEFAULT NULL,
  `dolgozoID` int(11) DEFAULT NULL,
  `ugyfelID` int(11) DEFAULT NULL,
  `foglalasStart` datetime DEFAULT NULL,
  `foglalasEnd` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `szolgáltatás`
--

CREATE TABLE `szolgáltatás` (
  `szolgaltatasID` int(11) NOT NULL,
  `szolgaltatasKategoria` varchar(255) DEFAULT NULL,
  `szolgaltatasIdotartam` time DEFAULT NULL,
  `szolgaltatasAr` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `szolgáltatás`
--

INSERT INTO `szolgáltatás` (`szolgaltatasID`, `szolgaltatasKategoria`, `szolgaltatasIdotartam`, `szolgaltatasAr`) VALUES
(1, 'Hajvágás', '01:15:00', 5000),
(2, 'Masszázs', '01:30:00', 10000),
(3, 'Manikűr', '01:45:00', 7000),
(4, 'Pedikűr', '01:45:00', 8000),
(5, 'Arckezelés', '00:30:00', 12000),
(6, 'Smink', '00:30:00', 8000),
(7, 'Tetoválás', '01:45:00', 15000),
(8, 'Fodrászat', '01:00:00', 6000),
(9, 'Szempilla', '00:30:00', 9000),
(10, 'Bőrkezelés', '00:45:00', 11000);

-- --------------------------------------------------------

--
-- Table structure for table `ügyfél`
--

CREATE TABLE `ügyfél` (
  `ugyfelID` int(11) NOT NULL,
  `ugyfelFirstName` varchar(255) DEFAULT NULL,
  `ugyfelLastName` varchar(255) DEFAULT NULL,
  `ugyfelTel` varchar(20) DEFAULT NULL,
  `ugyfelEmail` varchar(255) DEFAULT NULL,
  `ugyfelPontok` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `ügyfél`
--

INSERT INTO `ügyfél` (`ugyfelID`, `ugyfelFirstName`, `ugyfelLastName`, `ugyfelTel`, `ugyfelEmail`, `ugyfelPontok`) VALUES
(1, 'Gábor', 'Kovács', '+3612345678', 'gabor.kovacs@example.com', 120),
(2, 'Anna', 'Nagy', '+3622345678', 'anna.nagy@example.com', 200),
(3, 'Béla', 'Tóth', '+3632345678', 'bela.toth@example.com', 75),
(4, 'Zoltán', 'Varga', '+3642345678', 'zoltan.varga@example.com', 50),
(5, 'Eszter', 'Szabó', '+3652345678', 'eszter.szabo@example.com', 300),
(6, 'Krisztián', 'Horváth', '+3619876543', 'krisztian.horvath@example.com', 150),
(7, 'Zsófia', 'Molnár', '+3621987654', 'zsofia.molnar@example.com', 180),
(8, 'Ádám', 'Kiss', '+3631987654', 'adam.kiss@example.com', 220),
(9, 'Réka', 'Szilágyi', '+3641987654', 'reka.szilagyi@example.com', 90),
(10, 'László', 'Balogh', '+3651987654', 'laszlo.balogh@example.com', 130);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `dolgozók`
--
ALTER TABLE `dolgozók`
  ADD PRIMARY KEY (`dolgozoID`);

--
-- Indexes for table `foglalás`
--
ALTER TABLE `foglalás`
  ADD PRIMARY KEY (`foglalasID`),
  ADD KEY `szolgaltatasID` (`szolgaltatasID`),
  ADD KEY `dolgozoID` (`dolgozoID`),
  ADD KEY `ugyfelID` (`ugyfelID`);

--
-- Indexes for table `szolgáltatás`
--
ALTER TABLE `szolgáltatás`
  ADD PRIMARY KEY (`szolgaltatasID`);

--
-- Indexes for table `ügyfél`
--
ALTER TABLE `ügyfél`
  ADD PRIMARY KEY (`ugyfelID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `dolgozók`
--
ALTER TABLE `dolgozók`
  MODIFY `dolgozoID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `foglalás`
--
ALTER TABLE `foglalás`
  MODIFY `foglalasID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `szolgáltatás`
--
ALTER TABLE `szolgáltatás`
  MODIFY `szolgaltatasID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `ügyfél`
--
ALTER TABLE `ügyfél`
  MODIFY `ugyfelID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `foglalás`
--
ALTER TABLE `foglalás`
  ADD CONSTRAINT `foglalás_ibfk_1` FOREIGN KEY (`szolgaltatasID`) REFERENCES `szolgáltatás` (`szolgaltatasID`),
  ADD CONSTRAINT `foglalás_ibfk_2` FOREIGN KEY (`dolgozoID`) REFERENCES `dolgozók` (`dolgozoID`),
  ADD CONSTRAINT `foglalás_ibfk_3` FOREIGN KEY (`ugyfelID`) REFERENCES `ügyfél` (`ugyfelID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
