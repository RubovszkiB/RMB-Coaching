-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Feb 04. 10:17
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4
CREATE DATABASE IF NOT EXISTS fitness_app
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_hungarian_ci;

USE fitness_app;
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `fitness_app`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `coaches`
--

CREATE TABLE `coaches` (
  `coach_id` int(10) UNSIGNED NOT NULL,
  `full_name` varchar(120) NOT NULL,
  `email` varchar(180) NOT NULL,
  `phone` varchar(30) DEFAULT NULL,
  `bio` text DEFAULT NULL,
  `profile_image` varchar(500) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `is_active` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `coaches`
--

INSERT INTO `coaches` (`coach_id`, `full_name`, `email`, `phone`, `bio`, `profile_image`, `created_at`, `updated_at`, `is_active`) VALUES
(1, 'Rubovszki Balázs', 'balazs@fitnessapp.local', '+36 30 111 1111', 'Kondicionáló edző – izomépítés, szálkásítás, edzésterv + életmód.', NULL, '2026-02-04 08:08:21', '2026-02-04 08:08:21', 1),
(2, 'Bakaja Csaba', 'csaba@fitnessapp.local', '+36 30 222 2222', 'Labdarúgás-specifikus edző – gyorsaság, robbanékonyság, állóképesség, technika.', NULL, '2026-02-04 08:08:21', '2026-02-04 08:08:21', 1),
(3, 'Mezei Botond', 'botond@fitnessapp.local', '+36 30 333 3333', 'Erőnléti edző – strength & conditioning, teljesítményfokozás, atlétikai alapok.', NULL, '2026-02-04 08:08:21', '2026-02-04 09:18:45', 1);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `coaches`
--
ALTER TABLE `coaches`
  ADD PRIMARY KEY (`coach_id`),
  ADD UNIQUE KEY `uq_coaches_email` (`email`),
  ADD KEY `idx_coaches_active` (`is_active`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `coaches`
--
ALTER TABLE `coaches`
  MODIFY `coach_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
