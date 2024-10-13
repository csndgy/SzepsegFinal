-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Okt 04. 13:24
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `szepsegfinal`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `dolgozók`
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
-- A tábla adatainak kiíratása `dolgozók`
--

INSERT INTO `dolgozók` (`dolgozoID`, `dolgozoFirstName`, `dolgozoLastName`, `dolgozoTel`, `dolgozoEmail`, `statusz`, `szolgáltatasa`) VALUES
(1, 'Márk', 'Kiss', '+3612348765', 'mark.kiss@example.com', 1, 1),
(2, 'Sára', 'Horváth', '+3622348765', 'sara.horvath@example.com', 1, 2),
(3, 'János', 'Farkas', '+3632348765', 'janos.farkas@example.com', 0, 3),
(4, 'Dóra', 'Németh', '+3642348765', 'dora.nemeth@example.com', 1, 1),
(5, 'Ádám', 'Papp', '+3652348765', 'adam.papp@example.com', 0, 2);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `foglalás`
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
-- Tábla szerkezet ehhez a táblához `szolgáltatás`
--

CREATE TABLE `szolgáltatás` (
  `szolgaltatasID` int(11) NOT NULL,
  `szolgaltatasKategoria` varchar(255) DEFAULT NULL,
  `szolgaltatasIdotartam` datetime DEFAULT NULL,
  `szolgaltatasAr` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `szolgáltatás`
--

INSERT INTO `szolgáltatás` (`szolgaltatasID`, `szolgaltatasKategoria`, `szolgaltatasIdotartam`, `szolgaltatasAr`) VALUES
(1, 'Hajvágás', '0000-00-00 00:00:00', 5000),
(2, 'Masszázs', '0000-00-00 00:00:00', 10000),
(3, 'Manikűr', '0000-00-00 00:00:00', 7000),
(4, 'Pedikűr', '0000-00-00 00:00:00', 8000),
(5, 'Arckezelés', '0000-00-00 00:00:00', 12000);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `ügyfél`
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
-- A tábla adatainak kiíratása `ügyfél`
--

INSERT INTO `ügyfél` (`ugyfelID`, `ugyfelFirstName`, `ugyfelLastName`, `ugyfelTel`, `ugyfelEmail`, `ugyfelPontok`) VALUES
(1, 'Gábor', 'Kovács', '+3612345678', 'gabor.kovacs@example.com', 120),
(2, 'Anna', 'Nagy', '+3622345678', 'anna.nagy@example.com', 200),
(3, 'Béla', 'Tóth', '+3632345678', 'bela.toth@example.com', 75),
(4, 'Zoltán', 'Varga', '+3642345678', 'zoltan.varga@example.com', 50),
(5, 'Eszter', 'Szabó', '+3652345678', 'eszter.szabo@example.com', 300);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `dolgozók`
--
ALTER TABLE `dolgozók`
  ADD PRIMARY KEY (`dolgozoID`);

--
-- A tábla indexei `foglalás`
--
ALTER TABLE `foglalás`
  ADD PRIMARY KEY (`foglalasID`),
  ADD KEY `szolgaltatasID` (`szolgaltatasID`),
  ADD KEY `dolgozoID` (`dolgozoID`),
  ADD KEY `ugyfelID` (`ugyfelID`);

--
-- A tábla indexei `szolgáltatás`
--
ALTER TABLE `szolgáltatás`
  ADD PRIMARY KEY (`szolgaltatasID`);

--
-- A tábla indexei `ügyfél`
--
ALTER TABLE `ügyfél`
  ADD PRIMARY KEY (`ugyfelID`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `dolgozók`
--
ALTER TABLE `dolgozók`
  MODIFY `dolgozoID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `foglalás`
--
ALTER TABLE `foglalás`
  MODIFY `foglalasID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `szolgáltatás`
--
ALTER TABLE `szolgáltatás`
  MODIFY `szolgaltatasID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `ügyfél`
--
ALTER TABLE `ügyfél`
  MODIFY `ugyfelID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `foglalás`
--
ALTER TABLE `foglalás`
  ADD CONSTRAINT `foglalás_ibfk_1` FOREIGN KEY (`szolgaltatasID`) REFERENCES `szolgáltatás` (`szolgaltatasID`),
  ADD CONSTRAINT `foglalás_ibfk_2` FOREIGN KEY (`dolgozoID`) REFERENCES `dolgozók` (`dolgozoID`),
  ADD CONSTRAINT `foglalás_ibfk_3` FOREIGN KEY (`ugyfelID`) REFERENCES `ügyfél` (`ugyfelID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
